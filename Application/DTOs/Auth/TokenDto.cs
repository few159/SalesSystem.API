namespace SalesSystem.Application.DTOs.Auth;

public class TokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow + TimeSpan.FromHours(3);
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}