namespace SalesSystem.Application.Events;

public class ItemCancelledEvent
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public string Reason { get; set; } = null!;
    public DateTime CancelledAt { get; set; }
}