namespace AttendanceApp.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string MatriculationNumber { get; set; } = string.Empty;
        public string RegisteredDeviceId { get; set; } = string.Empty; // Anti-proxy measure
        public string? SelfieUrl { get; set; } // For visual verification if needed
    }
}