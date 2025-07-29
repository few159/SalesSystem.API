using MediatR;
using SalesSystem.Application.Events;
using SalesSystem.Application.Interfaces;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Application.Interfaces.Repositories.Base;

namespace SalesSystem.Application.Commands.Sale;

public class CancelSaleCommand : IRequest<bool>
{
    public Guid SaleId { get; set; }
}

public class CancelSaleCommandHandler(
    ISaleRepository saleRepository,
    IEventBus eventBus,
    IUnitOfWork uow
) : IRequestHandler<CancelSaleCommand, bool>
{
    public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        await uow.BeginTransactionAsync();
        try
        {
            var sale = await saleRepository.GetAsync(request.SaleId)
                       ?? throw new Exception("Venda não encontrada");

            sale.CancelSale();

            await saleRepository.UpdateAsync(sale);

            await eventBus.PublishAsync(new SaleCancelledEvent
            {
                SaleId = sale.Id,
                CancelledAt = DateTime.UtcNow
            });

            await uow.CommitAsync();
            await uow.SaveAsync();
            return true;
        }
        catch (Exception e)
        {
            await uow.RollBackAsync();
            throw;
        }
    }
}