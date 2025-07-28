using MongoDB.Driver;
using SalesSystem.Application.Interfaces.Repositories.Stores;
using SalesSystem.Domain.Enitities.Snapshot;

namespace SalesSystem.Infrastructure.Repositories.Stores;

public class ProductSnapshotStore : IProductSnapshotStore
{
    private readonly IMongoCollection<ProductSnapshot> _collection;

    public ProductSnapshotStore(IMongoDbContext context)
    {
        _collection = context.ProductSnapshots;
    }

    public async Task<ProductSnapshot?> GetAsync(Guid id)
    {
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProductSnapshot>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }
}