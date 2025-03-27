using LMS.Features.CourseManagement;
using System.ComponentModel.DataAnnotations;


namespace LMS.Features.CourseManagement.Dtos
{
    public class AddCourseDto
    {
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
        
        public string Objective { get; set; }
        public string Notes { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
