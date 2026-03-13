namespace AttendanceApp.Mobile.Models
{
    public class AuthResponse
    {
        public Guid StudentId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
    }
}