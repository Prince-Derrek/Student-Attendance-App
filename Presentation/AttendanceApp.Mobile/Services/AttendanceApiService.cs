using System.Net.Http.Json;
using AttendanceApp.Application.DTOs; // You may need to duplicate the DTO class in the mobile project or use a shared library. For now, assume it matches the backend DTO.

namespace AttendanceApp.Mobile.Services
{
    public class AttendanceApiService
    {
        private readonly HttpClient _httpClient;

        // Note: We will use a placeholder base address. In Phase 6, we will point this to Azure.
        // If testing on an Android Emulator locally, you would use "http://10.0.2.2:PORT"
        public AttendanceApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.edutrackpro.com/");
        }

        public async Task<bool> SubmitAttendanceAsync(Guid studentId, Guid sessionId, double lat, double lon, string deviceId)
        {
            var request = new AttendanceRequestDto
            {
                StudentId = studentId,
                LectureSessionId = sessionId,
                Latitude = lat,
                Longitude = lon,
                DeviceId = deviceId
            };

            // In a real scenario, you'd attach the JWT token to the _httpClient.DefaultRequestHeaders here
            var response = await _httpClient.PostAsJsonAsync("api/attendance/sign-in", request);

            return response.IsSuccessStatusCode;
        }
    }
}