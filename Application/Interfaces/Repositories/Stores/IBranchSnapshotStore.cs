using SalesSystem.Domain.Enitities.Snapshot;

namespace SalesSystem.Application.Interfaces.Repositories.Stores;

public interface IBranchSnapshotStore
{
    Task<BranchSnapshot?> GetAsync(string id);
    Task<IEnumerable<BranchSnapshot>> GetAllAsync();
}