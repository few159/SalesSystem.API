namespace Application.DTOs;

public class CreateSaleRequest
{
    public string CustomerId { get; set; } = string.Empty;
    public string BranchId { get; set; } = string.Empty;
    public List<CreateSaleItemRequest> Items { get; set; } = [];
}

public class CreateSaleItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}