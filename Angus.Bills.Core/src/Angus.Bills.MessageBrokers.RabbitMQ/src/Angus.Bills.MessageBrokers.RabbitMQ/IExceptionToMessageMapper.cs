using System;

namespace Angus.Bills.MessageBrokers.RabbitMQ
{
    public interface IExceptionToMessageMapper
    {
        object Map(Exception exception, object message);
    }
}