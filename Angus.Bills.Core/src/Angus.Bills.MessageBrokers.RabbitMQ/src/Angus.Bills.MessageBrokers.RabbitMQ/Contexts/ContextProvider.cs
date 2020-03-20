using System.Collections.Generic;
using System.Text;

namespace Angus.Bills.MessageBrokers.RabbitMQ.Contexts
{
    internal sealed class ContextProvider : IContextProvider
    {
        private readonly IRabbitMqSerializer _serializer;

        public ContextProvider(IRabbitMqSerializer serializer, RabbitMqOptions options)
        {
            _serializer = serializer;
            HeaderName = string.IsNullOrWhiteSpace(options.Context?.Header)
                ? "message_context"
                : options.Context.Header;
        }

        public string HeaderName { get; }

        public object Get(IDictionary<string, object> headers)
        {
            if (!headers.TryGetValue(HeaderName, out var context)) return null;

            if (!(context is byte[] bytes)) return null;

            return _serializer.Deserialize(Encoding.UTF8.GetString(bytes));
        }
    }
}