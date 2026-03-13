namespace AttendanceApp.Application.Interfaces
{
    public interface IGeoLocationService
    {
        bool IsWithinGeofence(double userLat, double userLon, double targetLat, double targetLon, double radiusInMeters);
    }
}