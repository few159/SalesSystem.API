using MediatR;
using SalesSystem.Application.Events;
using SalesSystem.Application.Interfaces;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Application.Interfaces.Repositories.Base;

namespace SalesSystem.Application.Commands.Carts;

public class CancelCartCommand : IRequest<bool>
{
    public Guid CartId { get; set; }
}

public class CancelCartCommandHandler(
    ICartRepository cartRepository,
    IEventBus eventBus,
    IUnitOfWork uow
) : IRequestHandler<CancelCartCommand, bool>
{
    public async Task<bool> Handle(CancelCartCommand request, CancellationToken cancellationToken)
    {
        await uow.BeginTransactionAsync();
        try
        {
            var cart = await cartRepository.GetAsync(request.CartId)
                       ?? throw new Exception("Carrinho não encontrado");

            cart.CancelCart();

            await cartRepository.UpdateAsync(cart);

            await eventBus.PublishAsync(new CartCancelledEvent
            {
                CartId = cart.Id,
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