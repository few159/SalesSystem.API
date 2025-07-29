using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Rebus.Config;
using SalesSystem.API.Middlewares;
using SalesSystem.Application.Commands.Sale;
using SalesSystem.Application.Mappings;
using SalesSystem.Infrastructure;
using SalesSystem.Infrastructure.Persistence;
using SalesSystem.Infrastructure.Repositories.Seeders;

var builder = WebApplication.CreateBuilder(args);

string corsPolicyName = "MainPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesSystem", Version = "v1" });
        // opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        // {
        //     In = ParameterLocation.Header,
        //     Description = "Please enter token",
        //     Name = "Authorization",
        //     Type = SecuritySchemeType.Http,
        //     BearerFormat = "JWT",
        //     Scheme = "bearer"
        // });
        //
        // opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        // {
        //     {
        //         new OpenApiSecurityScheme
        //         {
        //             Reference = new OpenApiReference
        //             {
        //                 Type = ReferenceType.SecurityScheme,
        //                 Id = "Bearer"
        //             }
        //         },
        //         new string[] { }
        //     }
        // });
    });

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddDbContext<SalesDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"))
);

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateSaleCommand).Assembly));
builder.Services.AddAutoMapper(cfg => { }, typeof(SaleProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed Data
new SeederRun(app.Services);

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseRouting();

app.UseCors(corsPolicyName);

app.MapControllers();

// Starta o Rebus
app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Services.StartRebus();
});

app.Run();