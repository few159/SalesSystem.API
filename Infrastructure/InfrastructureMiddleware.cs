using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Base;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public static class InfrastructureMiddleware
{
    internal static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISaleRepository, SaleRepository>();
    }
    
    internal static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddServices(configuration);
    }
}