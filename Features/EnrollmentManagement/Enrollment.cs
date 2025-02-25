using Dynamic_RBAMS.Features.CourseManagement;
using System.Globalization;

namespace Dynamic_RBAMS.Features.EnrollmentManagement
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public bool IsApproved { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        public string Grade { get; set; }

        public string Status { get; set; }

        public int AttendancePercentage { get; set; }

        // Foriegn keys and Navigation Properties are defiend here 
        public int CourseSectionId { get; set; }
        public CourseSection CourseSection { get; set; }
    }
}
