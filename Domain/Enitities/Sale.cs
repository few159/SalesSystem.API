namespace Domain.Enitities;

public class Sale
{
    public Guid Id { get; private set; }
    public string SaleNumber { get; private set; }
    public DateTime Date { get; private set; }

    // External Identities
    public string CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public string BranchId { get; private set; }
    public string BranchName { get; private set; }

    public decimal TotalAmount => Items.Sum(item => item.Total);
    public bool IsCancelled { get; private set; }

    public List<SaleItem> Items { get; private set; } = [];

    public Sale(string customerId, string customerName, string branchId, string branchName)
    {
        Id = Guid.NewGuid();
        SaleNumber = GenerateSaleNumber();
        Date = DateTime.UtcNow;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
    }

    public void AddItem(Guid productId, string productTitle, decimal unitPrice, int quantity)
    {
        if (quantity > 20)
            throw new InvalidOperationException("Não é possível vender mais de 20 unidades do mesmo produto.");

        decimal discount = 0;
        if (quantity >= 10)
            discount = 0.20m;
        else if (quantity >= 4)
            discount = 0.10m;

        if (quantity < 4 && discount > 0)
            throw new InvalidOperationException("Descontos não são permitidos para menos de 4 unidades.");

        Items.Add(new SaleItem(productId, productTitle, unitPrice, quantity, discount));
    }

    public void CancelItem(Guid productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId && !i.IsCancelled);
        if (item == null) throw new InvalidOperationException("Item não encontrado ou já cancelado.");

        item.Cancel();
    }

    public void CancelSale()
    {
        IsCancelled = true;
        foreach (var item in Items)
        {
            item.Cancel();
        }
    }

    private static string GenerateSaleNumber()
    {
        return $"SALE-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
    }
}