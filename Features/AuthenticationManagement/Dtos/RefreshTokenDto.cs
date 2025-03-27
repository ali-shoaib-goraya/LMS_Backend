using System.ComponentModel.DataAnnotations;

namespace LMS.Features.AuthenticationManagement.Dtos
{
    public class RefreshTokenDto
    {
        [Required]
        public string refreshToken { get; set; }
    }
}
