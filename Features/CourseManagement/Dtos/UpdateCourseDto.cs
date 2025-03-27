using System.ComponentModel.DataAnnotations;

namespace LMS.Features.CourseManagement.Dtos
{
    public class UpdateCourseDto
    {
        public string? CourseName { get; set; }

        public string? CourseCode { get; set; }

        public int? CreditHours { get; set; }
        public bool? IsLab { get; set; }
        public bool? IsCompulsory { get; set; }
        public bool? IsTheory { get; set; }

        public int? ConnectedCourseId { get; set; }    // Self-referencing foreign key

        public string? Objective { get; set; }
        public string? Notes { get; set; }
    }
}
