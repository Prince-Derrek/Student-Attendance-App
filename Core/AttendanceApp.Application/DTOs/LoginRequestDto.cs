namespace AttendanceApp.Application.DTOs
{
    public class LoginRequestDto
    {
        public string StudentIdNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}