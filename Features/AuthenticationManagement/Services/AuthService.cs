using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LMS.Data;
using LMS.Features.AuthenticationManagement.Dtos;
using LMS.Features.AuthenticationManagement.Services;
using LMS.Features.AuthenticationManagment;
using LMS.Features.Common.Dtos;
using LMS.Features.RoleManagement;
using LMS.Features.UserManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;

    private const int AccessTokenExpirySeconds = 1800; // 30 minutes
    private const int RefreshTokenExpiryDays = 7;

    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _dbContext = dbContext;
    }

    private string GenerateRefreshToken() => Guid.NewGuid().ToString();
    private async Task SaveRefreshTokenAsync(ApplicationUser user, string refreshToken, DateTime expiry)
    {
        _dbContext.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiryDate = expiry,
            IsRevoked = false
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task<AuthResponseDto> LoginUserAsync(string email, string password, string userType)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email && u.Type == userType);
        if (user == null)
            return new AuthResponseDto { Message = "Invalid email or password." };

        if (!await _userManager.CheckPasswordAsync(user, password))
            return new AuthResponseDto { Message = "Invalid email or password." };

        await RevokeAllUserTokensAsync(user.Id);

        var accessToken = await GenerateJwtTokenAsync(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays);

        await SaveRefreshTokenAsync(user, refreshToken, refreshTokenExpiry);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            AccessTokenExpiry = DateTime.UtcNow.AddSeconds(AccessTokenExpirySeconds),
            RefreshToken = refreshToken,
            RefreshTokenExpiry = refreshTokenExpiry,
            Message = "Login successful."
        };
    }

    public async Task<AuthResponseDto> RefreshTokensAsync(string refreshToken)
    {
        var (user, isValid, expiry) = await ValidateRefreshTokenAsync(refreshToken);
        if (!isValid || DateTime.UtcNow > expiry)
            return new AuthResponseDto { Message = "Invalid or expired refresh token." };

        var accessToken = await GenerateJwtTokenAsync(user);
        var newRefreshToken = GenerateRefreshToken();
        var newRefreshTokenExpiry = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays);

        await SaveRefreshTokenAsync(user, newRefreshToken, newRefreshTokenExpiry);
        await RevokeRefreshTokenAsync(refreshToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            AccessTokenExpiry = DateTime.UtcNow.AddSeconds(AccessTokenExpirySeconds),
            RefreshToken = newRefreshToken,
            RefreshTokenExpiry = newRefreshTokenExpiry,
            Message = "Token refreshed successfully."
        };
    }

    public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var expiryTime = DateTime.UtcNow.AddSeconds(AccessTokenExpirySeconds);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        new Claim("Type", user.Type ?? string.Empty),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        // 🔹 Fetch roles from DB and add them to claims
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // 🔹 Add UniversityId & CampusId **only for Faculty**
        if (user is Faculty facultyUser)
        {
            int? campusId = await _dbContext.FacultiesCampuses
                .Where(fc => fc.FacultyId == facultyUser.Id)
                .Select(fc => (int?)fc.CampusId)  // 👈 Make CampusId nullable
                .FirstOrDefaultAsync();

            claims.Add(new Claim("UniversityId", facultyUser.UniversityId?.ToString() ?? string.Empty));
            claims.Add(new Claim("CampusId", campusId?.ToString() ?? string.Empty));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiryTime,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public async Task<(ApplicationUser? user, bool isValid, DateTime expiry)> ValidateRefreshTokenAsync(string refreshToken)
    {
        var tokenRecord = await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        return tokenRecord != null && !tokenRecord.IsRevoked
            ? (tokenRecord.User, true, tokenRecord.ExpiryDate)
            : (null, false, DateTime.MinValue);
    }

    public async Task RevokeAllUserTokensAsync(string userId)
    {
        await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ExecuteUpdateAsync(s => s.SetProperty(rt => rt.IsRevoked, true));
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        await _dbContext.RefreshTokens
            .Where(rt => rt.Token == refreshToken)
            .ExecuteUpdateAsync(s => s.SetProperty(rt => rt.IsRevoked, true));
    }

    public async Task<(bool IsSuccessful, string Message)> LogoutAsync(string refreshToken)
    {
        await RevokeRefreshTokenAsync(refreshToken);
        return (true, "User logged out successfully.");
    }

    public async Task<ApplicationUser?> GetUserAsync(string token)
    {
        var principal = await GetPrincipalFromExpiredTokenAsync(token);
        var userId = principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        return string.IsNullOrEmpty(userId) ? null : await _userManager.FindByIdAsync(userId);
    }

    public Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return Task.FromResult(principal);
        }
        catch
        {
            return Task.FromResult(new ClaimsPrincipal());
        }
    }


}
