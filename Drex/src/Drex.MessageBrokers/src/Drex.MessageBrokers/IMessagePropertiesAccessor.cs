namespace Drex.MessageBrokers
{
    public interface IMessagePropertiesAccessor
    {
        IMessageProperties MessageProperties { get; set; }
    }
}