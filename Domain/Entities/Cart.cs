namespace SalesSystem.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public bool IsCancelled { get; private set; }
    public List<CartItem> Products { get; set; } = new();
  
    public Cart()
    {
        Id = Guid.NewGuid();
        Date = DateTime.UtcNow;
    }

    public void AddProducts(Guid productId, int quantity)
    {
        Products.Add(new CartItem(productId, quantity));
    }

    public void CancelProduct(Guid productId)
    {
        var products = Products.FirstOrDefault(i => i.ProductId == productId && !i.IsCancelled);
        if (products == null) throw new InvalidOperationException("Item não encontrado ou já cancelado.");

        products.Cancel();
    }

    public void CancelCart()
    {
        IsCancelled = true;
        foreach (var item in Products)
        {
            item.Cancel();
        }
    }

}
