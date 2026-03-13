using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AttendanceApp.Infrastructure.Data;
using AttendanceApp.Application.Interfaces;
using AttendanceApp.Application.DTOs;
using AttendanceApp.Domain.Entities;
using AttendanceApp.Domain.Enums;

namespace AttendanceApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires a valid JWT to access any endpoint here
    public class AttendanceController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IGeoLocationService _geoLocation;

        public AttendanceController(AppDbContext context, IGeoLocationService geoLocation)
        {
            _context = context;
            _geoLocation = geoLocation;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] AttendanceRequestDto request)
        {
            // 1. Validate Session & Classroom
            var session = await _context.LectureSessions
                .Include(ls => ls.Classroom)
                .FirstOrDefaultAsync(ls => ls.Id == request.LectureSessionId);

            if (session == null || !session.IsActive)
                return BadRequest("Invalid or inactive lecture session.");

            if (session.Classroom == null)
                return StatusCode(500, "Classroom data is missing for this session.");

            // 2. Validate Student & Anti-Proxy (Device ID check)
            var student = await _context.Students.FindAsync(request.StudentId);
            if (student == null)
                return NotFound("Student not found.");

            if (student.RegisteredDeviceId != request.DeviceId)
                return Unauthorized("Device mismatch. Proxy attendance is strictly prohibited.");

            // 3. Verify Geofence (Haversine logic)
            bool isWithinGeofence = _geoLocation.IsWithinGeofence(
                userLat: request.Latitude,
                userLon: request.Longitude,
                targetLat: session.Classroom.CenterLatitude,
                targetLon: session.Classroom.CenterLongitude,
                radiusInMeters: session.Classroom.GeofenceRadiusInMeters
            );

            // 4. Record Attendance
            var record = new AttendanceRecord
            {
                Id = Guid.NewGuid(),
                StudentId = request.StudentId,
                LectureSessionId = request.LectureSessionId,
                Timestamp = DateTime.UtcNow,
                SignedLatitude = request.Latitude,
                SignedLongitude = request.Longitude,
                IsVerified = isWithinGeofence,
                Status = isWithinGeofence ? AttendanceStatus.Present : AttendanceStatus.Absent
            };

            // 5. Prevent Duplicate Sign-ins
            var existingRecord = await _context.AttendanceRecords
                .AnyAsync(a => a.StudentId == request.StudentId && a.LectureSessionId == request.LectureSessionId);

            if (existingRecord)
                return Conflict("Attendance has already been recorded for this session.");

            _context.AttendanceRecords.Add(record);
            await _context.SaveChangesAsync();

            if (!isWithinGeofence)
                return BadRequest("Attendance rejected. You are outside the designated classroom area.");

            return Ok(new { Message = "Attendance recorded successfully.", RecordId = record.Id });
        }
    }
}