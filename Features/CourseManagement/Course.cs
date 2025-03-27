using LMS.Features.DepartmentManagement;
using System.ComponentModel.DataAnnotations;

namespace LMS.Features.CourseManagement
{
    public class Course
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; }

        [Required]
        public string CourseCode { get; set; }

        [Required]
        public int CreditHours { get; set; }

        [Required]
        public bool IsLab { get; set; }
        [Required]
        public bool IsCompulsory { get; set; }
        [Required]
        public bool IsTheory { get; set; }

        public int? ConnectedCourseId { get; set; }    // Self-referencing foreign key
        public Course ConnectedCourse { get; set; }    // Navigation property

        public string Objective { get; set; }
        public string Notes { get; set; }

        public bool IsDeleted { get; set; }

        // Forign keys and Navigation Properties are defined here 
        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
