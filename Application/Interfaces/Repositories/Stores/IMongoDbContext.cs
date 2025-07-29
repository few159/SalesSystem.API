using MongoDB.Driver;
using SalesSystem.Domain.Entities.Snapshot;

namespace SalesSystem.Application.Interfaces.Repositories.Stores;

public interface IMongoDbContext
{
    IMongoCollection<ProductSnapshot> ProductSnapshots { get; }
    IMongoCollection<CustomerSnapshot> CustomerSnapshots { get; }
    IMongoCollection<BranchSnapshot> BranchSnapshots { get; }
}