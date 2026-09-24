using PortfolioApi.Models;

namespace PortfolioApi.Services.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(AdminUser user);
}
