using LMS.Features.AuthenticationManagement.Dtos;
using LMS.Features.AuthenticationManagement.Services;
using LMS.Features.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            return BadRequest(new ErrorResponseDto
            {
                StatusCode = 400,
                Message = "Validation failed.",
                Errors = errors
            });
        }

        var response = await _authService.LoginUserAsync(request.Email, request.Password, request.Type);

        if (response.AccessToken == null)
        {
            return Unauthorized(new ApiResponseDto(401, "Invalid email or password."));
        }

        return Ok(new ApiResponseDto(200, "Login successful.", response));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto request)
    {
        var response = await _authService.RefreshTokensAsync(request.refreshToken);

        if (response.AccessToken == null)
        {
            return Unauthorized(new ApiResponseDto(401, "Invalid or expired refresh token."));
        }

        return Ok(new ApiResponseDto(200, "Token refreshed successfully.", response));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
    {
        var result = await _authService.LogoutAsync(request.RefreshToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(new ApiResponseDto(400, result.Message));
        }

        return Ok(new ApiResponseDto(200, "Successfully logged out."));
    }
}
