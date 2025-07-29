namespace SalesSystem.Application.DTOs.Cart;

public class CartDto
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartItemDto> Products { get; set; } = [];
}

public class CartItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}