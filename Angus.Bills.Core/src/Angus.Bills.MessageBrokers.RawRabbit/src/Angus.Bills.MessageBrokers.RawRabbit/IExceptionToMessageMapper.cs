using System;

namespace Angus.Bills.MessageBrokers.RawRabbit
{
    public interface IExceptionToMessageMapper
    {
        object Map(Exception exception, object message);
    }
}