using System.ComponentModel.DataAnnotations;
namespace LMS.Features.AuthenticationManagement.Dtos
{
    public class LoginDto
    {
        [Required] 
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Type { get; set; }
    }
}