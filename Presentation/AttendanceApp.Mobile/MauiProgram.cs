using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using CommunityToolkit.Maui;
using AttendanceApp.Mobile.Services;
using AttendanceApp.Mobile.ViewModels; // Added for AuthViewModel
using AttendanceApp.Mobile.Views;      // Added for AuthPage

namespace AttendanceApp.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("AttendanceApp.Mobile.appsettings.json");

            if (stream != null)
            {
                var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
                builder.Configuration.AddConfiguration(config);
            }

            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();

            // ==========================================
            // DEPENDENCY INJECTION REGISTRATIONS
            // ==========================================

            // Register Services
            builder.Services.AddHttpClient<AuthService>();

            // Register ViewModels & Views
            builder.Services.AddTransient<AuthViewModel>();
            builder.Services.AddTransient<AuthPage>();

            // ==========================================

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}