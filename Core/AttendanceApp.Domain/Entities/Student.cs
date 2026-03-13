namespace AttendanceApp.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string StudentIdNumber { get; set; } = string.Empty;
        public string MatriculationNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Anti-proxy measures
        public string RegisteredDeviceId { get; set; } = string.Empty;
        public string? SelfieUrl { get; set; }
    }
}