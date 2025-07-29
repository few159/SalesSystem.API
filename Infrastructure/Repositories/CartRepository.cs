using MongoDB.Driver.Linq;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Domain.Entities;
using SalesSystem.Infrastructure.Persistence;
using SalesSystem.Infrastructure.Repositories.Base;

namespace SalesSystem.Infrastructure.Repositories;

public class CartRepository(SalesDbContext context) : Repository<Cart>(context), ICartRepository
{
    public async Task<Cart> GetAsync(Guid id)
    {
        return await Query().FirstOrDefaultAsync(x => x.Id == id);
    }
}