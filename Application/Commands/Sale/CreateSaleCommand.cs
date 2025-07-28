using AutoMapper;
using MediatR;
using SalesSystem.Application.DTOs.Sale;
using SalesSystem.Application.Events;
using SalesSystem.Application.Interfaces;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Application.Interfaces.Repositories.Base;
using SalesSystem.Application.Interfaces.Repositories.Stores;
using SalesSystem.Domain.Enitities;
using SalesSystem.Domain.ValueObjects.Enums;

namespace SalesSystem.Application.Commands.Sales;

public class CreateSaleCommand : IRequest<SaleDto>
{
    public string CustomerId { get; set; } = string.Empty;
    public string BranchId { get; set; } = string.Empty;
    public SaleType SaleType { get; set; } = SaleType.Quotation;
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}

public class CreateSaleCommandHandler(
    IMapper mapper,
    ISaleRepository saleRepository,
    IProductSnapshotStore productSnapshotStore,
    ICustomerSnapshotStore customerSnapshotStore,
    IBranchSnapshotStore branchSnapshotStore,
    IEventBus eventBus,
    IUnitOfWork uow
) : IRequestHandler<CreateSaleCommand, SaleDto>
{
    public async Task<SaleDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        await uow.BeginTransactionAsync();
        try
        {
            // Buscar snapshots
            var customer = await customerSnapshotStore.GetAsync(request.CustomerId)
                           ?? throw new Exception("Cliente não encontrado");

            var branch = await branchSnapshotStore.GetAsync(request.BranchId)
                         ?? throw new Exception("Filial não encontrada");

            var sale = new Sale(request.CustomerId, customer.Name, request.BranchId, branch.Name, request.SaleType);

            foreach (var item in request.Items)
            {
                var product = await productSnapshotStore.GetAsync(item.ProductId)
                              ?? throw new Exception($"Produto {item.ProductId} não encontrado");

                sale.AddItem(product.Id, product.Title, product.Price, item.Quantity);
            }

            await saleRepository.InsertAsync(sale);

            await eventBus.PublishAsync(new SaleCreatedEvent
            {
                SaleId = sale.Id,
                CustomerId = sale.CustomerId,
                BranchId = sale.BranchId,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = sale.TotalAmount
            });

            await uow.CommitAsync();
            await uow.SaveAsync();

            return mapper.Map<SaleDto>(sale);
        }
        catch (Exception e)
        {
            await uow.RollBackAsync();
            throw;
        }
    }
}