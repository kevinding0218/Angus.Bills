using System;
using System.Collections.Generic;

namespace Angus.Bills.MessageBrokers.RabbitMQ
{
    public interface IConventionsRegistry
    {
        void Add<T>(IConventions conventions);
        void Add(Type type, IConventions conventions);
        IConventions Get<T>();
        IConventions Get(Type type);
        IEnumerable<IConventions> GetAll();
    }
}