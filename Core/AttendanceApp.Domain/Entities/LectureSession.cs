namespace AttendanceApp.Domain.Entities
{
    public class LectureSession
    {
        public Guid Id { get; set; }
        public Guid ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}