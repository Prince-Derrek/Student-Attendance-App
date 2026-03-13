namespace AttendanceApp.Domain.Entities
{
    public class Classroom
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
        public double GeofenceRadiusInMeters { get; set; }
    }
}