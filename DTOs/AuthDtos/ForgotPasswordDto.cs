using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.DTOs.AuthDtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
