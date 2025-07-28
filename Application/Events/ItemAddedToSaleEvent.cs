namespace SalesSystem.Application.Events;

public class ItemAddedToSaleEvent
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; }
}