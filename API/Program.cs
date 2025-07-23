using Application.Mappings;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SalesSystem.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

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
    opt.UseNpgsql("Host=127.0.0.1:5433;Username=SA_User;Password=adm1234#;Database=SalesSystem")
);

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddAutoMapper(cfg => { }, typeof(SaleProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseRouting();

app.MapControllers();

app.Run();