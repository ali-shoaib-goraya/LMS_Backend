using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.DTOs.AuthDtos
{
    public class RefreshTokenDto
    {
        [Required]
        public string refreshToken { get; set; }
    }
}
 