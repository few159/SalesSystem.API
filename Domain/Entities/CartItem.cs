namespace SalesSystem.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public bool IsCancelled { get; private set; }

    public CartItem(Guid productId, int quantity)
    {
        ProductId = Guid.NewGuid();
        Quantity = quantity;
    }

    public void Cancel()
    {
        IsCancelled = true;
    }
}
