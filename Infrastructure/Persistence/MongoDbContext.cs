using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SalesSystem.Application.Interfaces.Repositories.Stores;
using SalesSystem.Domain.Entities.Snapshot;

namespace SalesSystem.Infrastructure.Persistence;

public class MongoDbContext: IMongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var mongoUrl = configuration.GetConnectionString("MongoDb");
        var mongoClient = new MongoClient(mongoUrl);

        _database = mongoClient.GetDatabase("DeveloperStoreSnapshots");
    }
    
    public IMongoCollection<ProductSnapshot> ProductSnapshots =>
        _database.GetCollection<ProductSnapshot>("productsnapshot");

    public IMongoCollection<CustomerSnapshot> CustomerSnapshots =>
        _database.GetCollection<CustomerSnapshot>("customersnapshot");

    public IMongoCollection<BranchSnapshot> BranchSnapshots =>
        _database.GetCollection<BranchSnapshot>("branchsnapshot");
}