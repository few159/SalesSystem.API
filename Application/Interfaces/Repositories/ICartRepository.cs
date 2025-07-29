using SalesSystem.Application.Interfaces.Repositories.Base;
using SalesSystem.Domain.Enitities;

namespace SalesSystem.Application.Interfaces.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart> GetAsync(Guid id);
}