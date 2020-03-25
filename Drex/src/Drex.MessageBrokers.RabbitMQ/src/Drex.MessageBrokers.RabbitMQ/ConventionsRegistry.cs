using System;
using System.Collections.Generic;

namespace Drex.MessageBrokers.RabbitMQ
{
    public class ConventionsRegistry : IConventionsRegistry
    {
        private readonly IDictionary<Type, IConventions> _conventions = new Dictionary<Type, IConventions>();

        public void Add<T>(IConventions conventions)
        {
            Add(typeof(T), conventions);
        }

        public void Add(Type type, IConventions conventions)
        {
            _conventions[type] = conventions;
        }

        public IConventions Get<T>()
        {
            return Get(typeof(T));
        }

        public IConventions Get(Type type)
        {
            return _conventions.TryGetValue(type, out var conventions) ? conventions : null;
        }

        public IEnumerable<IConventions> GetAll()
        {
            return _conventions.Values;
        }
    }
}