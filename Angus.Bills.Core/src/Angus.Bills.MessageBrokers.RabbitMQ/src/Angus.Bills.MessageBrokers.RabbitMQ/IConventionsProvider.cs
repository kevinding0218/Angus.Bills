using System;

namespace Angus.Bills.MessageBrokers.RabbitMQ
{
    public interface IConventionsProvider
    {
        IConventions Get<T>();
        IConventions Get(Type type);
    }
}