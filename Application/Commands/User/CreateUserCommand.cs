using AutoMapper;
using MediatR;
using SalesSystem.Application.DTOs.User;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Application.Interfaces.Repositories.Base;
using SalesSystem.Application.Interfaces.Services;

namespace SalesSystem.Application.Commands.User;

public class CreateUserCommand : CreateUserRequest, IRequest<UserDto>
{
}

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IHasher hasher,
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await uow.BeginTransactionAsync();
        try
        {
            request.Password = hasher.HashPassword(request.Password);
            var newUser = mapper.Map<Domain.Enitities.User>(request);
            await userRepository.InsertAsync(newUser);

            await uow.CommitAsync();
            await uow.SaveAsync();
            return mapper.Map<UserDto>(newUser);
        }
        catch (Exception e)
        {
            await uow.RollBackAsync();
            throw;
        }
    }
}