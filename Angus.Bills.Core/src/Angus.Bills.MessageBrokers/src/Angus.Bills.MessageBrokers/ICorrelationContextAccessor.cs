namespace Angus.Bills.MessageBrokers
{
    public interface ICorrelationContextAccessor
    {
        object CorrelationContext { get; set; }
    }
}