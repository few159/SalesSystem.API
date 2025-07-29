namespace SalesSystem.Application.Events;

public class CartCreatedEvent
{
    public Guid CartId { get; set; }
    public string CustomerId { get; set; } = null!;
    public string BranchId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
}