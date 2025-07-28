namespace SalesSystem.Application.Events;

public class SaleCancelledEvent
{
    public Guid SaleId { get; set; }
    public DateTime CancelledAt { get; set; }
}