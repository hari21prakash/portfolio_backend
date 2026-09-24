using PortfolioApi.DTOs.Auth;

namespace PortfolioApi.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}
