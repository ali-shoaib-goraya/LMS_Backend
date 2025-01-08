using Dynamic_RBAMS.DTOs.AuthDtos;
using Dynamic_RBAMS.Interfaces;
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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

        var (isSuccessful, errorMessage) = await _authService.RegisterUserAsync(request.Email, request.Password, request.Type);
        if (!isSuccessful)
            return BadRequest(new { success = false, message = errorMessage });

        return Ok(new { success = true, message = "User registered successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

        var response = await _authService.LoginUserAsync(request.Email, request.Password);
        if (response.AccessToken == null)
            return Unauthorized(new { success = false, message = response.Message });

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto request)
    {
        var response = await _authService.RefreshTokensAsync(request.Token);
        if (response.AccessToken == null)
            return Unauthorized(new { success = false, message = response.Message });

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
    {
        var result = await _authService.LogoutAsync(request.RefreshToken);

        if (!result.IsSuccessful)
            return BadRequest(new { Message = result.ErrorMessage });

        return Ok(new { Message = "Successfully logged out." });
    }

}
