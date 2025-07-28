using Microsoft.EntityFrameworkCore;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Domain.Enitities;
using SalesSystem.Infrastructure.Persistence;
using SalesSystem.Infrastructure.Repositories.Base;

namespace SalesSystem.Infrastructure.Repositories;

public class SaleRepository(SalesDbContext context) : Repository<Sale>(context), ISaleRepository
{
    public async Task<Sale> GetAsync(Guid id)
    {
        return await Query()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
}