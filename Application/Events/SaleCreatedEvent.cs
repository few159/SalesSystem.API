namespace SalesSystem.Application.Events;

public class SaleCreatedEvent
{
    public Guid SaleId { get; set; }
    public string CustomerId { get; set; } = null!;
    public string BranchId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
}