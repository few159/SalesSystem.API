using SalesSystem.Domain.Enitities;
using SalesSystem.Domain.ValueObjects.Enums;
using SalesSystem.Infrastructure.Persistence;

namespace SalesSystem.Infrastructure.Repositories.Seeders;

public static class UserSeeder
{
    public static async Task SeedAsync(SalesDbContext context)
    {
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User()
                {
                    Email = "admin@gmail.com",
                    Password = "abc123",
                    Username = "Admin",
                    Status = UserStatus.Active,
                    Role = UserRole.Admin,
                    Name = { Firstname = "Admin", Lastname = "User" },
                }
            };
        }
    }
}