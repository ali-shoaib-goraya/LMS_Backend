using System.ComponentModel.DataAnnotations;

namespace LMS.Features.AuthenticationManagement.Dtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
