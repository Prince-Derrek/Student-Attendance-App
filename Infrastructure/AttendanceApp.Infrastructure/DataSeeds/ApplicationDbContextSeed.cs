using AttendanceApp.Domain.Entities; // Adjust namespace to match your domain
using AttendanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApp.Infrastructure.DataSeeds
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // 1. Seed the Mock Student
            if (!await context.Students.AnyAsync(s => s.StudentIdNumber == "20240589"))
            {
                var student = new Student
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    FirstName = "Derrek",
                    LastName = "Kimani",
                    StudentIdNumber = "20240589",
                    Email = "derrekkinyanjui@proton.me",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!")
                };
                await context.Students.AddAsync(student);
            }

            // 2. Seed a Classroom / Geofence (Juja Coordinates)
            var classroomId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            if (!await context.Classrooms.AnyAsync(c => c.Id == classroomId))
            {
                var classroom = new Classroom
                {
                    Id = classroomId,
                    Name = "Science Block 302",
                    CenterLatitude = -1.1018,
                    CenterLongitude = 37.0144,
                    GeofenceRadiusInMeters = 1000
                };
                await context.Classrooms.AddAsync(classroom);
            }

            // 3. Seed an Active Lecture Session
            var sessionId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            if (!await context.LectureSessions.AnyAsync(ls => ls.Id == sessionId))
            {
                var session = new LectureSession
                {
                    Id = sessionId,
                    ClassroomId = classroomId,
                    CourseName = "Software Architecture",
                    StartTime = DateTime.UtcNow.AddDays(-1),
                    EndTime = DateTime.UtcNow.AddDays(1),
                    IsActive = true
                };
                await context.LectureSessions.AddAsync(session);
            }

            // Save all changes to the database
            await context.SaveChangesAsync();
        }
    }
}