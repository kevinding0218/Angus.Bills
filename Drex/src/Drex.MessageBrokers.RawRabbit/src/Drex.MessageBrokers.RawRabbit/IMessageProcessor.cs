using System.Threading.Tasks;

namespace Drex.MessageBrokers.RawRabbit
{
    public interface IMessageProcessor
    {
        Task<bool> TryProcessAsync(string id);
        Task RemoveAsync(string id);
    }
}