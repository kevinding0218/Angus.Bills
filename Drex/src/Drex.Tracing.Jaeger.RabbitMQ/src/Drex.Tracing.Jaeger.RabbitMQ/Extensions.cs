using Drex.MessageBrokers.RabbitMQ;
using Drex.Tracing.Jaeger.RabbitMQ.Plugins;

namespace Drex.Tracing.Jaeger.RabbitMQ
{
    public static class Extensions
    {
        public static IRabbitMqPluginsRegistry AddJaegerRabbitMqPlugin(this IRabbitMqPluginsRegistry registry)
        {
            registry.Add<JaegerPlugin>();
            return registry;
        }
    }
}