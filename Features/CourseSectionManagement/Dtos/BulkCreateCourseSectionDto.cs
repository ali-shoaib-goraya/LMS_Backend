using System.ComponentModel.DataAnnotations;

namespace LMS.Features.CourseSectionManagement.Dtos
{
    public class BulkCreateCourseSectionDto
    {
        [Required(ErrorMessage = "SchoolId is required")]
        public int SchoolId { get; set; }

        [Required(ErrorMessage = "SemesterId is required")]
        public int SemesterId { get; set; }

        public string? Prefix { get; set; }

        [Required(ErrorMessage = "CourseSections list cannot be empty")]
        public List<CreateCourseSectionItemDto> CourseSections { get; set; }
    }

    public class CreateCourseSectionItemDto
    {
        [Required(ErrorMessage = "CourseSectionName is required")]
        public string CourseSectionName { get; set; }

        public string? Status { get; set; } = "Open";
        public string? Section { get; set; }
       
        public string? Notes { get; set; }

        [Required(ErrorMessage = "FacultyId is required")]
        public string FacultyId { get; set; }

        [Required(ErrorMessage = "CourseId is required")]
        public int CourseId { get; set; }
    }
}
