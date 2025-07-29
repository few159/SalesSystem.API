using SalesSystem.Domain.Entities.Snapshot;

namespace SalesSystem.Application.Interfaces.Repositories.Stores;

public interface IBranchSnapshotStore
{
    Task<BranchSnapshot?> GetAsync(string id);
    Task<IEnumerable<BranchSnapshot>> GetAllAsync();
}