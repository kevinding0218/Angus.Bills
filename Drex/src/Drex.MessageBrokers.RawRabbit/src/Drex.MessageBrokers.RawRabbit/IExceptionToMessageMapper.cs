using System;

namespace Drex.MessageBrokers.RawRabbit
{
    public interface IExceptionToMessageMapper
    {
        object Map(Exception exception, object message);
    }
}