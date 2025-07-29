using SalesSystem.Application.Interfaces.Repositories.Base;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Interfaces.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    Task<Sale> GetAsync(Guid id);
}