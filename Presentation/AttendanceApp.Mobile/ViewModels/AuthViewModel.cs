using AttendanceApp.Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AttendanceApp.Mobile.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty]
        private string _studentIdNumber = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public AuthViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        public async Task SignInAsync()
        {
            if (string.IsNullOrWhiteSpace(StudentIdNumber) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both ID and Password.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            bool isSuccess = await _authService.LoginAsync(StudentIdNumber, Password);

            IsBusy = false;

            if (isSuccess)
            {
                // Clear the password from memory
                Password = string.Empty;

                // Navigate to the main AppShell or the Attendance page
                // Assuming your route to the main app is "//MainPage" or similar
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                ErrorMessage = "Invalid ID or Password.";
            }
        }
    }
}