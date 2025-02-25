using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Features.AuthenticationManagement.Dtos
{
    public class RefreshTokenDto
    {
        [Required]
        public string refreshToken { get; set; }
    }
}
