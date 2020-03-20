using System.Threading.Tasks;

namespace Angus.Bills.CQRS.Events
{
    public interface IEventDispatcher
    {
        Task PublishAsync<T>(T @event) where T : class, IEvent;
    }
}