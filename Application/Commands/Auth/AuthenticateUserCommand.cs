using MediatR;
using SalesSystem.Application.DTOs.Auth;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Application.Interfaces.Services;

namespace SalesSystem.Application.Commands.Auth;

public class AuthenticateUserCommand : IRequest<TokenDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthenticateUserCommandHandler(
    IUserRepository userRepository,
    IHasher hasher,
    IJwtTokenService jwtTokenService
) : IRequestHandler<AuthenticateUserCommand, TokenDto>
{
    public async Task<TokenDto> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.Email)
                   ?? throw new Exception("Usuário ou senha inválidos");

        if (!hasher.VerifyPassword(request.Password, user.Password))
            throw new Exception("Usuário ou senha inválidos");

        var token = jwtTokenService.GenerateToken(user);

        return new TokenDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(3),
            Email = user.Email
        };
    }
}