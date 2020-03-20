using System.Threading.Tasks;
using Angus.Bills.CQRS.Commands;
using Angus.Bills.CQRS.Events;

namespace Angus.Bills.MessageBrokers.CQRS.Dispatchers
{
    internal sealed class ServiceBusMessageDispatcher : ICommandDispatcher, IEventDispatcher
    {
        private readonly ICorrelationContextAccessor _accessor;
        private readonly IBusPublisher _busPublisher;

        public ServiceBusMessageDispatcher(IBusPublisher busPublisher, ICorrelationContextAccessor accessor)
        {
            _busPublisher = busPublisher;
            _accessor = accessor;
        }

        public Task SendAsync<T>(T command) where T : class, ICommand
        {
            return _busPublisher.SendAsync(command, _accessor.CorrelationContext);
        }

        public Task PublishAsync<T>(T @event) where T : class, IEvent
        {
            return _busPublisher.PublishAsync(@event, _accessor.CorrelationContext);
        }
    }
}