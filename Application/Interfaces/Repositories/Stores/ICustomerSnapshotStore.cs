using SalesSystem.Domain.Entities.Snapshot;

namespace SalesSystem.Application.Interfaces.Repositories.Stores;

public interface ICustomerSnapshotStore
{
    Task<CustomerSnapshot?> GetAsync(string id);
    Task<IEnumerable<CustomerSnapshot>> GetAllAsync();
}