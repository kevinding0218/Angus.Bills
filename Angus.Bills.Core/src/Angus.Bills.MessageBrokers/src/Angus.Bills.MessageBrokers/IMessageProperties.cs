using System.Collections.Generic;

namespace Angus.Bills.MessageBrokers
{
    public interface IMessageProperties
    {
        string MessageId { get; }
        string CorrelationId { get; }
        long Timestamp { get; }
        IDictionary<string, object> Headers { get; }
    }
}