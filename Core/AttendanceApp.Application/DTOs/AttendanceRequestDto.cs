namespace AttendanceApp.Application.DTOs
{
    public class AttendanceRequestDto
    {
        public Guid StudentId { get; set; }
        public Guid LectureSessionId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DeviceId { get; set; } = string.Empty;
    }
}