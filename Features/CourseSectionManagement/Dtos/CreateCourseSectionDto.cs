using System.ComponentModel.DataAnnotations;

namespace LMS.Features.CourseSectionManagement.Dtos
{
    public class CreateCourseSectionDto
    {
        [Required(ErrorMessage = "CourseSectionName is Required")]
        public string CourseSectionName { get; set; }
        public string? Status { get; set; } = "Open";
        public string? Section { get; set; }
        public string? Prefix { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;


        [Required(ErrorMessage = "FacultyId is Required")]
        public string FacultyId { get; set; }

        [Required(ErrorMessage = "SemesterId is Required")]
        public int SemesterId { get; set; }

        [Required(ErrorMessage = "CourseId is Required")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "SchoolId is Required")]
        public int SchoolId { get; set; }
    }
}
