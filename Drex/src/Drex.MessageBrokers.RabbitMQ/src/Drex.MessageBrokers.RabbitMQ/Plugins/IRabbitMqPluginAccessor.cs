using System;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace Drex.MessageBrokers.RabbitMQ.Plugins
{
    internal interface IRabbitMqPluginAccessor
    {
        void SetSuccessor(Func<object, object, BasicDeliverEventArgs, Task> successor);
    }
}