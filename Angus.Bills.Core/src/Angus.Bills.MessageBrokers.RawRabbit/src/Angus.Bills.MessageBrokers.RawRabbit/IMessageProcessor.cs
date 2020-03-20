using System.Threading.Tasks;

namespace Angus.Bills.MessageBrokers.RawRabbit
{
    public interface IMessageProcessor
    {
        Task<bool> TryProcessAsync(string id);
        Task RemoveAsync(string id);
    }
}