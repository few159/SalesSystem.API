namespace SalesSystem.Application.Events;

public class CartCancelledEvent
{
    public Guid CartId { get; set; }
    public DateTime CancelledAt { get; set; }
}