using System;

namespace Angus.Bills.MessageBrokers.RabbitMQ.Plugins
{
    internal sealed class RabbitMqPluginChain
    {
        public Type PluginType { get; set; }
        public IRabbitMqPlugin Plugin { get; set; }
    }
}