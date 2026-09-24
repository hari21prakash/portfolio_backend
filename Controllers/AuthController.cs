using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs.Auth;
using PortfolioApi.Services.Interfaces;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Invalid request payload."));

        var result = await _authService.LoginAsync(request);

        if (result is null)
            return Unauthorized(ApiResponse<object>.Fail("Invalid username or password."));

        return Ok(ApiResponse<LoginResponseDto>.Ok(result, "Login successful."));
    }
}
