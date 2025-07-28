using AutoMapper;
using MediatR;
using SalesSystem.Application.DTOs.Sale;
using SalesSystem.Application.Events;
using SalesSystem.Application.Interfaces;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Application.Interfaces.Repositories.Stores;

namespace SalesSystem.Application.Commands.Item;

public class AddSaleItemCommand : IRequest<SaleDto>
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class AddSaleItemCommandHandler(
    ISaleRepository saleRepository,
    IProductSnapshotStore productSnapshotStore,
    IEventBus eventBus,
    IMapper mapper
) : IRequestHandler<AddSaleItemCommand, SaleDto>
{
    public async Task<SaleDto> Handle(AddSaleItemCommand request, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetAsync(request.SaleId)
                   ?? throw new Exception("Venda não encontrada");

        var product = await productSnapshotStore.GetAsync(request.ProductId)
                      ?? throw new Exception("Produto não encontrado");

        sale.AddItem(product.Id, product.Title, product.Price, request.Quantity);

        await saleRepository.UpdateAsync(sale);

        await eventBus.PublishAsync(new ItemAddedToSaleEvent
        {
            SaleId = sale.Id,
            ProductId = product.Id,
            Quantity = request.Quantity,
            AddedAt = DateTime.UtcNow
        });

        return mapper.Map<SaleDto>(sale);
    }
}