using System.Net.Http.Json;
using AttendanceApp.Mobile.Models;
using Microsoft.Extensions.Configuration;

namespace AttendanceApp.Mobile.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            string baseUrl = _configuration["ApiSettings:BaseUrl"];
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<bool> LoginAsync(string studentIdNumber, string password)
        {
            var request = new LoginRequest
            {
                StudentIdNumber = studentIdNumber,
                Password = password
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var authData = await response.Content.ReadFromJsonAsync<AuthResponse>();

                    if (authData != null)
                    {
                        // Securely store the token, Student ID, and First Name for later use
                        await SecureStorage.Default.SetAsync("jwt_token", authData.Token);
                        await SecureStorage.Default.SetAsync("student_id", authData.StudentId.ToString());
                        await SecureStorage.Default.SetAsync("first_name", authData.FirstName);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle network errors (e.g., log them)
                Console.WriteLine($"Login failed: {ex.Message}");
            }

            return false;
        }
    }
}