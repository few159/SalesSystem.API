using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rebus.Config;
using SalesSystem.Application.Interfaces;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Application.Interfaces.Repositories.Base;
using SalesSystem.Application.Interfaces.Repositories.Stores;
using SalesSystem.Application.Interfaces.Services;
using SalesSystem.Infrastructure.MessageBus;
using SalesSystem.Infrastructure.MessageBus.Handlers;
using SalesSystem.Infrastructure.Persistence;
using SalesSystem.Infrastructure.Repositories;
using SalesSystem.Infrastructure.Repositories.Base;
using SalesSystem.Infrastructure.Repositories.Stores;
using SalesSystem.Infrastructure.Services;

namespace SalesSystem.Infrastructure;

public static class InfrastructureMiddleware
{
    internal static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
    }

    internal static void AddMongoContext(this IServiceCollection services)
    {
        services.AddSingleton<IMongoDbContext, MongoDbContext>();
    }

    internal static void AddMongoStores(this IServiceCollection services)
    {
        services.AddScoped<IProductSnapshotStore, ProductSnapshotStore>();
        services.AddScoped<IBranchSnapshotStore, BranchSnapshotStore>();
        services.AddScoped<ICustomerSnapshotStore, CustomerSnapshotStore>();
    }

    internal static void AddEvents(this IServiceCollection services, IConfiguration configuration)
    {
        var test = configuration.GetConnectionString("RabbitMQ");
        services.AddRebus(configure => configure.Transport(t =>
            t.UseRabbitMq(configuration.GetConnectionString("RabbitMQ"), "SalesSystem")
        ));

        services.AddScoped<IEventBus, EventBus>();

        services.AutoRegisterHandlersFromAssemblyOf<SaleCreatedEventHandler>();
    }

    internal static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IHasher, Hasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
    }

    internal static void SetAuhenticaion(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]!);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
    }

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddMongoContext();
        services.AddMongoStores();
        services.AddEvents(configuration);
        services.AddServices(configuration);
        services.SetAuhenticaion(configuration);
    }
}