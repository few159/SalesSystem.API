using MongoDB.Driver;
using SalesSystem.Application.Interfaces.Repositories.Stores;
using SalesSystem.Domain.Entities.Snapshot;

namespace SalesSystem.Infrastructure.Repositories.Stores;

public class BranchSnapshotStore : IBranchSnapshotStore
{
    private readonly IMongoCollection<BranchSnapshot> _collection;

    public BranchSnapshotStore(IMongoDbContext context)
    {
        _collection = context.BranchSnapshots;
    }

    public async Task<BranchSnapshot?> GetAsync(string id)
    {
        return await _collection.Find(b => b.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<BranchSnapshot>> GetAllAsync()
    {
        return await _collection.Find(FilterDefinition<BranchSnapshot>.Empty).ToListAsync();
    }
}