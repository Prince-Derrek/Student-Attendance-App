using AttendanceApp.Application.DTOs;
using AttendanceApp.Application.Interfaces;
using AttendanceApp.Domain.Entities;
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
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // 1. Check if the student ID is already taken
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentIdNumber == request.StudentIdNumber);

            if (existingStudent != null)
            {
                return BadRequest(new { Message = "A student with this ID already exists." });
            }

            // 2. Hash the new password using your BCrypt service
            string hashedPassword = _passwordHasher.Hash(request.Password);

            // 3. Create the new Student entity
            var newStudent = new Student
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                StudentIdNumber = request.StudentIdNumber,
                Email = request.Email,
                PasswordHash = hashedPassword
            };

            // 4. Save to Neon Database
            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            // 5. Generate a Token so they are immediately authenticated
            string token = _jwtProvider.Generate(newStudent);

            var response = new AuthResponseDto
            {
                StudentId = newStudent.Id,
                Token = token,
                FirstName = newStudent.FirstName
            };

            return Ok(response);
        }
    }
}