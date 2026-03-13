using AttendanceApp.Application.Interfaces;
using System;

namespace AttendanceApp.Infrastructure.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        public bool IsWithinGeofence(double userLat, double userLon, double targetLat, double targetLon, double radiusInMeters)
        {
            var R = 6371e3; // Earth's radius in meters
            var phi1 = userLat * Math.PI / 180;
            var phi2 = targetLat * Math.PI / 180;
            var deltaPhi = (targetLat - userLat) * Math.PI / 180;
            var deltaLambda = (targetLon - userLon) * Math.PI / 180;

            var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                    Math.Cos(phi1) * Math.Cos(phi2) *
                    Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;

            return distance <= radiusInMeters;
        }
    }
}