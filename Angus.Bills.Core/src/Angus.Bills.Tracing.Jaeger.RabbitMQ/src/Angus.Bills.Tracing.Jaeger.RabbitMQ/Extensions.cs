using Angus.Bills.MessageBrokers.RabbitMQ;
using Angus.Bills.Tracing.Jaeger.RabbitMQ.Plugins;

namespace Angus.Bills.Tracing.Jaeger.RabbitMQ
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