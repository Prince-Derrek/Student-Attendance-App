using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AttendanceApp.Mobile.Services;

namespace AttendanceApp.Mobile.ViewModels
{
    public partial class AttendanceVerificationViewModel : ObservableObject
    {
        private readonly AttendanceApiService _apiService;

        [ObservableProperty]
        private bool _isProcessing;

        [ObservableProperty]
        private string _statusMessage = "Ready to verify.";

        public AttendanceVerificationViewModel(AttendanceApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        public async Task CaptureAndVerifyAsync()
        {
            IsProcessing = true;
            StatusMessage = "Checking Location...";

            try
            {
                // 1. Get High-Accuracy GPS Location
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location == null)
                {
                    StatusMessage = "Failed to get GPS location.";
                    return;
                }

                StatusMessage = "Location acquired. Opening Camera...";

                // 2. Capture Selfie (Anti-Proxy measure 1)
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo == null)
                    {
                        StatusMessage = "Selfie capture cancelled.";
                        return; // User cancelled taking the photo
                    }
                    // In a production app, you would upload this photo stream to Azure Blob Storage here
                }

                StatusMessage = "Verifying with server...";

                // 3. Get Unique Device ID (Anti-Proxy measure 2)
                // We generate a GUID on first launch and store it securely in Preferences
                string deviceId = Preferences.Default.Get("RegisteredDeviceId", string.Empty);
                if (string.IsNullOrEmpty(deviceId))
                {
                    deviceId = Guid.NewGuid().ToString();
                    Preferences.Default.Set("RegisteredDeviceId", deviceId);
                }

                // 4. Submit to Backend API
                Guid mockStudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Mock for now
                Guid mockSessionId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Mock for now

                bool isSuccess = await _apiService.SubmitAttendanceAsync(mockStudentId, mockSessionId, location.Latitude, location.Longitude, deviceId);

                if (isSuccess)
                {
                    StatusMessage = "Attendance Marked Successfully!";
                }
                else
                {
                    StatusMessage = "Verification Failed. You may be outside the geofence or using an unregistered device.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsProcessing = false;
            }
        }
    }
}