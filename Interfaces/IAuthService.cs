using Dynamic_RBAMS.DTOs.AuthDtos;
using Dynamic_RBAMS.Models;
using System.Security.Claims;

namespace Dynamic_RBAMS.Interfaces
{
    public interface IAuthService
    {
        Task<(bool IsSuccessful, string ErrorMessage)> RegisterUserAsync(string email, string password, string type, string role, string first_name, string last_name);
        Task<AuthResponseDto> LoginUserAsync(string email, string password, string type); 
        Task<AuthResponseDto> RefreshTokensAsync(string refreshToken);

        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
        Task<ApplicationUser> GetUserAsync(string token);

        Task<(bool IsSuccessful, string ErrorMessage)> LogoutAsync(string refreshToken);
    }


}
 
