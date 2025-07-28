using AutoMapper;
using MediatR;
using SalesSystem.Application.DTOs.Sale;
using SalesSystem.Application.Events;
using SalesSystem.Application.Interfaces;
using SalesSystem.Application.Interfaces.Repositories;

namespace SalesSystem.Application.Commands.Item;

public class CancelSaleItemCommand : IRequest<SaleDto>
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
}

public class CancelSaleItemCommandHandler(
    ISaleRepository saleRepository,
    IEventBus eventBus,
    IMapper mapper
) : IRequestHandler<CancelSaleItemCommand, SaleDto>
{
    public async Task<SaleDto> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetAsync(request.SaleId)
                   ?? throw new Exception("Venda não encontrada");

        sale.CancelItem(request.ItemId);

        await saleRepository.UpdateAsync(sale);

        await eventBus.PublishAsync(new ItemCancelledEvent
        {
            SaleId = sale.Id,
            ItemId = request.ItemId,
            CancelledAt = DateTime.UtcNow
        });

        return mapper.Map<SaleDto>(sale);
    }
}