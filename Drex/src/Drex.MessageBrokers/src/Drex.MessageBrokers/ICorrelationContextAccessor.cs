namespace Drex.MessageBrokers
{
    public interface ICorrelationContextAccessor
    {
        object CorrelationContext { get; set; }
    }
}