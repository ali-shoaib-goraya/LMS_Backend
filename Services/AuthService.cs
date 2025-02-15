using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dynamic_RBAMS.Data;
using Dynamic_RBAMS.DTOs.AuthDtos;
using Dynamic_RBAMS.Interfaces;
using Dynamic_RBAMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;

    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _dbContext = dbContext;
        
    }

    public async Task<(bool IsSuccessful, string ErrorMessage)> RegisterUserAsync(string email, string password, string userType, string firstName, string lastName, string role)
    {
        // Convert string to UserType 
        if (!Enum.TryParse(userType, out UserType userTypeEnum))
        {
            return (false, "Invalid user type.");
        }
        // Check if role exists
        if (!await _roleManager.RoleExistsAsync(role))
        {
            return (false, "Role does not exist.");
        }
        // create user
        var user = new ApplicationUser { UserName = email, Email = email, Type = userTypeEnum.ToString(), FirstName = firstName, LastName = lastName };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "Registration failed.";
            return (false, errorMessage);
        }
        // Assign role to user
        await _userManager.AddToRoleAsync(user, role);
        return (true, string.Empty);
    }

    public async Task<AuthResponseDto> LoginUserAsync(string email, string password, string userType)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return new AuthResponseDto { Message = "Invalid email or password." };

        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!isValidPassword)
            return new AuthResponseDto { Message = "Invalid email or password." };

        if (user.Type != userType)
            return new AuthResponseDto { Message = "Invalid email or password" };
        

        // Revoke all existing refresh tokens for the user
        await RevokeAllUserTokensAsync(user.Id);


        var accessToken = await GenerateJwtTokenAsync(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await SaveRefreshTokenAsync(user, refreshToken, refreshTokenExpiry);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(30), // Match access token expiry
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
        var newRefreshTokenExpiry = DateTime.UtcNow.AddSeconds(60);

        await SaveRefreshTokenAsync(user, newRefreshToken, newRefreshTokenExpiry);
        await RevokeRefreshTokenAsync(refreshToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            AccessTokenExpiry = DateTime.UtcNow.AddSeconds(30), // Match access token expiry
            RefreshToken = newRefreshToken,
            RefreshTokenExpiry = newRefreshTokenExpiry,
            Message = "Token refreshed successfully."
        };
    }

    public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var expiryTime = DateTime.UtcNow.AddSeconds(50); // Access token expiry

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim("Email", user.Email ?? string.Empty),
            new Claim("UserId", user.Id.ToString()),
            new Claim("Type", user.Type ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiryTime,
            signingCredentials: creds
        );

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<ApplicationUser> GetUserAsync(string token)
    {
        var principal = await GetPrincipalFromExpiredTokenAsync(token);
        if (principal == null)
            throw new Exception("Invalid token");

        var userId = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new Exception("Invalid token");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        return user;
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
            ValidateLifetime = false // Allow expired tokens
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is JwtSecurityToken jwtToken &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return Task.FromResult(principal);
            }
        }
        catch (Exception)
        {
            // Log exception if necessary
        }

        return Task.FromResult<ClaimsPrincipal>(null);
    }


    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }


    private async Task RevokeAllUserTokensAsync(string userId)
    {
        var tokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
            token.IsRevoked = true;

        await _dbContext.SaveChangesAsync();
    }

    private async Task SaveRefreshTokenAsync(ApplicationUser user, string refreshToken, DateTime expiry)
    {
        var newRefreshToken = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiryDate = expiry,
            IsRevoked = false
        };

        _dbContext.Set<RefreshToken>().Add(newRefreshToken);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<(ApplicationUser? user, bool isValid, DateTime expiry)> ValidateRefreshTokenAsync(string refreshToken)
    {
        var tokenRecord = await _dbContext.Set<RefreshToken>()
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (tokenRecord == null || tokenRecord.IsRevoked || DateTime.UtcNow > tokenRecord.ExpiryDate)
            return (null, false, tokenRecord?.ExpiryDate ?? DateTime.MinValue);

        return (tokenRecord.User, true, tokenRecord.ExpiryDate);
    }

    private async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var tokenRecord = await _dbContext.Set<RefreshToken>()
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (tokenRecord != null)
        {
            tokenRecord.IsRevoked = true;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<(bool IsSuccessful, string ErrorMessage)> LogoutAsync(string refreshToken)
    {
        var tokenRecord = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (tokenRecord == null)
            return (false, "Invalid refresh token.");

        tokenRecord.IsRevoked = true; // Mark the token as revoked
        await _dbContext.SaveChangesAsync();

        return (true, string.Empty);
    }

}
