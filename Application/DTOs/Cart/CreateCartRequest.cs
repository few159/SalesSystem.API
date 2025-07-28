namespace SalesSystem.Application.DTOs.Cart;

public class CreateCartRequest
{
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartItemDto> Products { get; set; } = [];
}