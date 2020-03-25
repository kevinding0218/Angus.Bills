using System;
using System.Threading.Tasks;
using Drex.MessageBrokers.RabbitMQ.Plugins;
using RabbitMQ.Client.Events;

namespace Drex.MessageBrokers.RabbitMQ
{
    public abstract class RabbitMqPlugin : IRabbitMqPlugin, IRabbitMqPluginAccessor
    {
        private Func<object, object, BasicDeliverEventArgs, Task> _successor;

        public abstract Task HandleAsync(object message, object correlationContext,
            BasicDeliverEventArgs args);

        void IRabbitMqPluginAccessor.SetSuccessor(Func<object, object, BasicDeliverEventArgs, Task> successor)
        {
            _successor = successor;
        }

        public Task Next(object message, object correlationContext, BasicDeliverEventArgs args)
        {
            return _successor(message, correlationContext, args);
        }
    }
}