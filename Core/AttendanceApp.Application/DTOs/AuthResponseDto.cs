namespace AttendanceApp.Application.DTOs
{
    public class AuthResponseDto
    {
        public Guid StudentId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
    }
}