using Rebus.Bus;
using SalesSystem.Application.Interfaces;

namespace SalesSystem.Infrastructure.MessageBus;

public class EventBus(IBus _bus) : IEventBus
{
    public Task PublishAsync<T>(T @event) where T : class
    {
        return _bus.Publish(@event);
    }
}