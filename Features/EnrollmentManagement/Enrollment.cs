using LMS.Features.CourseSectionManagement.Models;
using LMS.Features.StudentManagement.Models;
using System.Globalization;

namespace LMS.Features.EnrollmentManagement 
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public bool? IsApproved { get; set; }

        public string? Grade { get; set; }

        public string? Status { get; set; } 

        public int? AttendancePercentage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } 

        // Foriegn keys and Navigation Properties are defiend here 
        public string StudentId { get; set; }
        public Student Student { get; set; }
        public int CourseSectionId { get; set; }
        public CourseSection CourseSection { get; set; }
    }
}
