using System;

namespace Drex.MessageBrokers.RabbitMQ
{
    public interface IExceptionToMessageMapper
    {
        object Map(Exception exception, object message);
    }
}