using Rebus.Handlers;
using SalesSystem.Application.Events;

namespace SalesSystem.Infrastructure.MessageBus.Handlers;

public class SaleCreatedEventHandler : IHandleMessages<SaleCreatedEvent>
{
    public Task Handle(SaleCreatedEvent message)
    {
        // Usado para implementar lógica de middleware para eventos, não necessário para essa aplicação
        throw new NotImplementedException();
    }
}