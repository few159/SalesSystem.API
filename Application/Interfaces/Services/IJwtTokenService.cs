using SalesSystem.Domain.Enitities;

namespace SalesSystem.Application.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}