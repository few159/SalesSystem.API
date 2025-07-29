using SalesSystem.Application.Interfaces.Repositories.Base;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Interfaces.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart> GetAsync(Guid id);
}