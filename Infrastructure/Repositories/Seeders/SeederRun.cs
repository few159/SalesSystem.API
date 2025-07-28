using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Application.Interfaces.Repositories.Stores;

namespace SalesSystem.Infrastructure.Repositories.Seeders;

public class SeederRun
{
    public SeederRun(IServiceProvider appServices)
    {
        var scope = appServices.CreateScope();
        var mongoContext = scope.ServiceProvider.GetRequiredService<IMongoDbContext>();
        
        RunSeedsAsync(mongoContext).GetAwaiter().GetResult();
    }

    private async Task RunSeedsAsync(IMongoDbContext ctx)
    {
        await ProductSnapshotSeeder.SeedAsync(ctx);
        await CustomerSnapshotSeeder.SeedAsync(ctx);
        await BranchSnapshotSeeder.SeedAsync(ctx);
    }
}