using SalesSystem.Domain.ValueObjects;
using SalesSystem.Domain.ValueObjects.Enums;

namespace SalesSystem.Application.DTOs.User;

public class CreateUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Name Name { get; set; } = new();
    public Address Address { get; set; } = new();
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = UserStatus.Active.ToString();
    public string Role { get; set; } = UserRole.Customer.ToString();
}