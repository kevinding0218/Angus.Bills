using System.Collections.Generic;

namespace Angus.Bills.MessageBrokers.RabbitMQ.Plugins
{
    internal interface IRabbitMqPluginsRegistryAccessor
    {
        LinkedList<RabbitMqPluginChain> Get();
    }
}