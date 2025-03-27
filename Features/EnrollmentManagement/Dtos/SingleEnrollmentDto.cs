using System.ComponentModel.DataAnnotations;

namespace LMS.Features.EnrollmentManagement.Dtos
{
    public class SingleEnrollmentDto
    {
        [Required(ErrorMessage = "StudentId is required.")]
        public string StudentId { get; set; }

        [Required(ErrorMessage = "CourseSectionId is required.")]
        public int CourseSectionId { get; set; }
    }
}
