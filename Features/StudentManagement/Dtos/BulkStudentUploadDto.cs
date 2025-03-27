using System.ComponentModel.DataAnnotations;

namespace LMS.Features.StudentManagement.Dtos
{
    public class BulkStudentUploadDto
    {
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string EnrollmentNo { get; set; }

        [Required]
        public string GuardianName { get; set; }

        [Required]
        public string GuardianContact { get; set; }
    }
}
