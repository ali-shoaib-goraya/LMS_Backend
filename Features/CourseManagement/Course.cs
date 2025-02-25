using Dynamic_RBAMS.Features.DepartmentManagement;

namespace Dynamic_RBAMS.Features.CourseManagement
{
    public class Course
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public string CourseCode { get; set; }

        public int CreditHours { get; set; }

        public bool IsLab { get; set; }
        public bool IsCompulsory { get; set; }

        public bool IsTheory { get; set; }

        public int? ConnectedCourseId { get; set; }    // Self-referencing foreign key
        public Course ConnectedCourse { get; set; }    // Navigation property

        public string Objective { get; set; }
        public string Notes { get; set; }

        // Forign keys and Navigation Properties are defined here 
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
