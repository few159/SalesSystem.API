using MongoDB.Driver;
using SalesSystem.Application.Interfaces.Repositories.Stores;
using SalesSystem.Domain.Entities.Snapshot;

namespace SalesSystem.Infrastructure.Repositories.Seeders;

public static class CustomerSnapshotSeeder
{
    public static async Task SeedAsync(IMongoDbContext context)
    {
        var collection = context.CustomerSnapshots;
        
        await collection.DeleteManyAsync(FilterDefinition<CustomerSnapshot>.Empty);

        var customers = new List<CustomerSnapshot>
        {
            new() { Id = "1001", Name = "João Silva" },
            new() { Id = "1002", Name = "Maria Oliveira" },
            new() { Id = "1003", Name = "Carlos Souza" },
            new() { Id = "1004", Name = "Ana Paula Rocha" },
            new() { Id = "1005", Name = "Fernanda Gomes" },
        };

        await collection.InsertManyAsync(customers);
    }
}