namespace AttendanceApp.Mobile.Models
{
    public class LoginRequest
    {
        public string StudentIdNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}