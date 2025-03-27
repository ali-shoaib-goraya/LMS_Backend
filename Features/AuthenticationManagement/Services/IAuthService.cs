using System.Security.Claims;
using LMS.Features.AuthenticationManagement.Dtos; 
using LMS.Features.UserManagement.Models;

namespace LMS.Features.AuthenticationManagement.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginUserAsync(string email, string password, string type);
        Task<AuthResponseDto> RefreshTokensAsync(string refreshToken);
        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
        Task<ApplicationUser?> GetUserAsync(string token);
        Task<(bool IsSuccessful, string Message)> LogoutAsync(string refreshToken);

        // 🔹 Added for better token handling across Faculty & Student
        Task<(ApplicationUser? user, bool isValid, DateTime expiry)> ValidateRefreshTokenAsync(string refreshToken);
        Task RevokeAllUserTokensAsync(string userId);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
