namespace SalesSystem.Domain.Enitities;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartItem> Products { get; set; } = new();
}
