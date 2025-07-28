using MongoDB.Driver;
using SalesSystem.Application.Interfaces.Repositories.Stores;
using SalesSystem.Domain.Enitities.Snapshot;

namespace SalesSystem.Infrastructure.Repositories.Stores;

public class CustomerSnapshotStore : ICustomerSnapshotStore
{
    private readonly IMongoCollection<CustomerSnapshot> _collection;

    public CustomerSnapshotStore(IMongoDbContext context)
    {
        _collection = context.CustomerSnapshots;
    }

    public async Task<CustomerSnapshot?> GetAsync(string id)
    {
        return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CustomerSnapshot>> GetAllAsync()
    {
        return await _collection.Find(FilterDefinition<CustomerSnapshot>.Empty).ToListAsync();
    }
}