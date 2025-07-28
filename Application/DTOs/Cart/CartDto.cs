namespace SalesSystem.Application.DTOs.Cart;

public class CartDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartItemDto> Products { get; set; } = [];
}

public class CartItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}