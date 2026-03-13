using AttendanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<LectureSession> LectureSessions { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Entity relationships and constraints
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.MatriculationNumber)
                .IsUnique();

            modelBuilder.Entity<AttendanceRecord>()
                .HasIndex(a => new { a.StudentId, a.LectureSessionId })
                .IsUnique(); // Prevents a student from signing in twice for the same session
        }
    }
}