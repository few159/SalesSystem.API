using SalesSystem.Domain.ValueObjects;
using SalesSystem.Domain.ValueObjects.Enums;

namespace SalesSystem.Domain.Enitities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public Name Name { get; set; } = new();
    public Address Address { get; set; } = new();
    public string Phone { get; set; } = string.Empty;
    public UserStatus Status { get; set; } = UserStatus.Active;
    public UserRole Role { get; set; } = UserRole.Customer;

    public bool ValidadePassword(string password) => password == Password;
}