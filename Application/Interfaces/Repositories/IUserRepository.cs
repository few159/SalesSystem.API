using SalesSystem.Application.Interfaces.Repositories.Base;
using SalesSystem.Domain.Enitities;

namespace SalesSystem.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetAsync(string email);
    Task<User> GetAsync(int id);
}