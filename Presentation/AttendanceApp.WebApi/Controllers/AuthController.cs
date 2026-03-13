using AttendanceApp.Application.DTOs;
using AttendanceApp.Application.Interfaces;
using AttendanceApp.Infrastructure; // Assuming DbContext is accessible here
using AttendanceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthController(
            AppDbContext context,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // 1. Find the student by their university ID
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentIdNumber == request.StudentIdNumber);

            if (student == null)
            {
                return Unauthorized(new { Message = "Invalid credentials." });
            }

            // 2. Verify the BCrypt password hash
            bool isPasswordValid = _passwordHasher.Verify(request.Password, student.PasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "Invalid credentials." });
            }

            // 3. Generate the JWT Token
            string token = _jwtProvider.Generate(student);

            // 4. Return the secure payload
            var response = new AuthResponseDto
            {
                StudentId = student.Id,
                Token = token,
                FirstName = student.FirstName
            };

            return Ok(response);
        }
    }
}