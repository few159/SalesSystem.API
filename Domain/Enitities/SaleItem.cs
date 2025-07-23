namespace Domain.Enitities;

public class SaleItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductTitle { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public bool IsCancelled { get; private set; }

    public decimal Total =>
        Math.Round((UnitPrice * Quantity) * (1 - Discount), 2);

    public SaleItem(Guid productId, string productTitle, decimal unitPrice, int quantity, decimal discount)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductTitle = productTitle;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Discount = discount;
    }

    public void Cancel()
    {
        IsCancelled = true;
    }
}