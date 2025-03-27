namespace LMS.Features.ActivityManagement.Helper
{
    public static class GradingHelper
    {
        public static (string Grade, decimal GPA) CalculateGPA(decimal percentage)
        {
            if (percentage >= 90) return ("A+", 4.00m);
            if (percentage >= 85) return ("A", 4.00m);
            if (percentage >= 80) return ("A-", 3.67m);
            if (percentage >= 75) return ("B+", 3.33m);
            if (percentage >= 70) return ("B", 3.00m);
            if (percentage >= 65) return ("B-", 2.67m);
            if (percentage >= 60) return ("C+", 2.33m);
            if (percentage >= 55) return ("C", 2.00m);
            return ("F", 0.00m); // Fail
        }
    }
}

