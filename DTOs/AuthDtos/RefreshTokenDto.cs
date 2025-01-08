using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.DTOs.AuthDtos
{
    public class RefreshTokenDto
    {
        [Required]
        public string Token { get; set; }
    }
}
