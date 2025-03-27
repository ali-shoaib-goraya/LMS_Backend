using System.ComponentModel.DataAnnotations;

namespace LMS.Features.EnrollmentManagement.Dtos
{
    public class BulkEnrollmentDto
    {
        [Required(ErrorMessage = "StudentIds is required.")]
        public List<string> StudentIds { get; set; }

        [Required(ErrorMessage = "CourseSectionId is required.")]
        public int CourseSectionId { get; set; }
    }
}
