using Microsoft.EntityFrameworkCore;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Domain.Enitities;
using SalesSystem.Infrastructure.Persistence;
using SalesSystem.Infrastructure.Repositories.Base;

namespace SalesSystem.Infrastructure.Repositories;

public class UserRepository(SalesDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User> GetAsync(string email)
    {
        return await Query()
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<User> GetAsync(int id)
    {
        return await Query()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
}