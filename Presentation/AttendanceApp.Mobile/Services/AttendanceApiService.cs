using System.Net.Http.Json;
using AttendanceApp.Application.DTOs;
using Microsoft.Extensions.Configuration; // You may need to duplicate the DTO class in the mobile project or use a shared library. For now, assume it matches the backend DTO.

namespace AttendanceApp.Mobile.Services
{
    public class AttendanceApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        // Note: We will use a placeholder base address. In Phase 6, we will point this to Azure.
        // If testing on an Android Emulator locally, you would use "http://10.0.2.2:PORT"
        public AttendanceApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            string baseUrl = _config["ApiSettings:BaseUrl"];
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<bool> SubmitAttendanceAsync(Guid sessionId, double lat, double lon, string deviceId)
        {
            // 1. Pull the actual logged-in user's ID and Token from the phone's vault
            string storedStudentIdStr = await SecureStorage.Default.GetAsync("student_id");
            string storedToken = await SecureStorage.Default.GetAsync("jwt_token");

            if (string.IsNullOrEmpty(storedStudentIdStr) || string.IsNullOrEmpty(storedToken))
            {
                return false; // User is not logged in
            }

            var request = new AttendanceRequestDto
            {
                StudentId = Guid.Parse(storedStudentIdStr), // Using the REAL ID!
                LectureSessionId = sessionId, // Keep the seeded session Guid for now: "22222222-2222-2222-2222-222222222222"
                Latitude = lat,
                Longitude = lon,
                DeviceId = deviceId
            };

            // 2. Attach the JWT Token to the Authorization header
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", storedToken);

            // 3. Fire the request
            var response = await _httpClient.PostAsJsonAsync("api/attendance/sign-in", request);

            return response.IsSuccessStatusCode;
        }
    }
}