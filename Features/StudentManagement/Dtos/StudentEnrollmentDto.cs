using System.ComponentModel.DataAnnotations;

namespace LMS.Features.StudentManagement.Dtos
{
    public class StudentEnrollmentDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string EnrollmentNo { get; set; }

        [Required]
        public string GuardianName { get; set; }

        [Required]
        public string GuardianContact { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int BatchSectionId { get; set; }
    }
}
