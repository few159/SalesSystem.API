using MongoDB.Driver;
using SalesSystem.Application.Interfaces.Repositories.Stores;
using SalesSystem.Domain.Enitities.Snapshot;

namespace SalesSystem.Infrastructure.Repositories.Seeders;

public static class BranchSnapshotSeeder
{
    public static async Task SeedAsync(IMongoDbContext context)
    {
        var collection = context.BranchSnapshots;

        await collection.DeleteManyAsync(FilterDefinition<BranchSnapshot>.Empty);

        var branches = new List<BranchSnapshot>
        {
            new() { Id = "RJ01", Name = "Filial Rio de Janeiro - Centro" },
            new() { Id = "SP01", Name = "Filial São Paulo - Paulista" },
            new() { Id = "MG01", Name = "Filial Belo Horizonte - Savassi" },
            new() { Id = "RS01", Name = "Filial Porto Alegre - Moinhos" },
            new() { Id = "SC01", Name = "Filial Florianópolis - Centro" },
        };

        await collection.InsertManyAsync(branches);
    }
}
