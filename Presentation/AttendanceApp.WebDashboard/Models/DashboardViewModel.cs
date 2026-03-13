namespace AttendanceApp.WebDashboard.Models
{
    public class DashboardViewModel
    {
        public string ProfessorName { get; set; } = "Professor's Hub";
        public string Department { get; set; } = "Computer Science & Mathematics";

        public ActiveSessionViewModel? ActiveSession { get; set; }
        public List<CourseMetricViewModel> Courses { get; set; } = new();
    }

    public class ActiveSessionViewModel
    {
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string TimeRemaining { get; set; } = string.Empty;
        public int StudentsPresent { get; set; }
        public int TotalEnrolled { get; set; }
        public string LocationName { get; set; } = string.Empty;
    }

    public class CourseMetricViewModel
    {
        public string CourseName { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public int AverageAttendancePercentage { get; set; }
        public string Icon { get; set; } = string.Empty;
    }
}