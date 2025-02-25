using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Features.AuthenticationManagement.Dtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
