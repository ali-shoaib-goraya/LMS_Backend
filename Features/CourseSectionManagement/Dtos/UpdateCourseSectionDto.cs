using System.ComponentModel.DataAnnotations;

namespace LMS.Features.CourseSectionManagement.Dtos
{
    public class UpdateCourseSectionDto
    {
        [Required(ErrorMessage ="CourseSectionName is Required"), MinLength(6, ErrorMessage = "CourseSectionName cannot be empty")]
        public string CourseSectionName { get; set; }

        [MaxLength(10, ErrorMessage = "Prefix cannot be more than 10 characters")]
        public string? Prefix { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public string? Section { get; set; }
        [Required(ErrorMessage ="FacultyId is Required")]
        public string FacultyId { get; set; }
    }
}
