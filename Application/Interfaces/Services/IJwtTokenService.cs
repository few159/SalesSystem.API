using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}