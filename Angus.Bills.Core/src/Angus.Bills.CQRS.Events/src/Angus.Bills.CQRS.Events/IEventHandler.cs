using System.Threading.Tasks;

namespace Angus.Bills.CQRS.Events
{
    public interface IEventHandler<in TEvent> where TEvent : class, IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}