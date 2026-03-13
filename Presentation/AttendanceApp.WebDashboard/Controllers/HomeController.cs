using AttendanceApp.WebDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AttendanceApp.WebDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                ActiveSession = new ActiveSessionViewModel
                {
                    CourseName = "Advanced Calculus",
                    CourseCode = "SMA-2104",
                    TimeRemaining = "45:12",
                    StudentsPresent = 42,
                    TotalEnrolled = 50,
                    LocationName = "Juja Main Campus - Science Block"
                },
                Courses = new List<CourseMetricViewModel>
                {
                    new CourseMetricViewModel
                    {
                        CourseName = "Data Structures",
                        Schedule = "Mondays, Wednesdays • 10:00 AM",
                        AverageAttendancePercentage = 82,
                        Icon = "science"
                    },
                    new CourseMetricViewModel
                    {
                        CourseName = "Object-Oriented Programming",
                        Schedule = "Tuesdays, Thursdays • 01:30 PM",
                        AverageAttendancePercentage = 95,
                        Icon = "auto_graph"
                    }
                }
            };

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
