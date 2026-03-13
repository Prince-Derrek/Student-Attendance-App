using AttendanceApp.Domain.Enums;

namespace AttendanceApp.Domain.Entities
{
    public class AttendanceRecord
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        public Guid LectureSessionId { get; set; }
        public LectureSession? LectureSession { get; set; }
        public DateTime Timestamp { get; set; }
        public AttendanceStatus Status { get; set; }
        public double SignedLatitude { get; set; }
        public double SignedLongitude { get; set; }
        public bool IsVerified { get; set; } // True if within geofence and device matches
    }
}