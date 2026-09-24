using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.DTOs.Auth;
using PortfolioApi.Services.Interfaces;

namespace PortfolioApi.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(ApplicationDbContext db, ITokenService tokenService, ILogger<AuthService> logger)
    {
        _db = db;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _db.AdminUsers
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        // Intentionally generic failure path (no "user not found" vs "wrong password" distinction)
        // to avoid leaking which usernames exist.
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Failed admin login attempt for username {Username}", request.Username);
            return null;
        }

        var (token, expiresAt) = _tokenService.GenerateToken(user);

        user.LastLoginAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            Username = user.Username,
            Email = user.Email
        };
    }
}
