using System;
using System.Linq;
using System.Reflection;

namespace Drex.MessageBrokers.RabbitMQ.Conventions
{
    public class ConventionsBuilder : IConventionsBuilder
    {
        private readonly RabbitMqOptions _options;
        private readonly bool _snakeCase;

        public ConventionsBuilder(RabbitMqOptions options)
        {
            _options = options;
            _snakeCase = options.ConventionsCasing?.Equals("snakeCase",
                StringComparison.InvariantCultureIgnoreCase) == true;
        }

        public string GetRoutingKey(Type type)
        {
            var attribute = GeAttribute(type);
            var routingKey = string.IsNullOrWhiteSpace(attribute?.RoutingKey) ? type.Name : attribute.RoutingKey;

            return WithCasing(routingKey);
        }

        public string GetExchange(Type type)
        {
            var attribute = GeAttribute(type);
            var exchange = string.IsNullOrWhiteSpace(attribute?.Exchange)
                ? string.IsNullOrWhiteSpace(_options.Exchange?.Name) ? type.Assembly.GetName().Name :
                _options.Exchange.Name
                : attribute.Exchange;

            return WithCasing(exchange);
        }

        public string GetQueue(Type type)
        {
            var attribute = GeAttribute(type);
            var exchange = string.IsNullOrWhiteSpace(attribute?.Exchange) ? _options.Exchange.Name : attribute.Exchange;
            exchange = string.IsNullOrWhiteSpace(exchange) ? string.Empty : $"{exchange}.";
            var queue = string.IsNullOrWhiteSpace(attribute?.Queue)
                ? $"{type.Assembly.GetName().Name}/{exchange}{type.Name}"
                : attribute.Queue;

            return WithCasing(queue);
        }

        private string WithCasing(string value)
        {
            return _snakeCase ? SnakeCase(value) : value;
        }

        private static string SnakeCase(string value)
        {
            return string.Concat(value.Select((x, i) =>
                    i > 0 && value[i - 1] != '.' && value[i - 1] != '/' && char.IsUpper(x) ? "_" + x : x.ToString()))
                .ToLowerInvariant();
        }

        private static MessageAttribute GeAttribute(MemberInfo type)
        {
            return type.GetCustomAttribute<MessageAttribute>();
        }
    }
}