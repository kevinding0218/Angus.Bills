using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Drex.Initializers;
using Drex.MessageBrokers.RawRabbit.Builders;
using Drex.MessageBrokers.RawRabbit.Processors;
using Drex.MessageBrokers.RawRabbit.Publishers;
using Drex.MessageBrokers.RawRabbit.Registers;
using Drex.MessageBrokers.RawRabbit.Subscribers;
using Drex.Persistence.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Instantiation;
using RawRabbit.Pipe;
using RawRabbit.Pipe.Middleware;

namespace Drex.MessageBrokers.RawRabbit
{
    public static class Extensions
    {
        private const string SectionName = "rabbitMq";
        private const string RegistryName = "messageBrokers.rabbitMq";

        internal static string GetMessageName(this object message)
        {
            return message.GetType().Name.Underscore().ToLowerInvariant();
        }

        public static IBusSubscriber UseRabbitMq(this IApplicationBuilder app)
        {
            return new BusSubscriber(app);
        }

        public static IDrexBuilder AddExceptionToMessageMapper<T>(this IDrexBuilder builder)
            where T : class, IExceptionToMessageMapper
        {
            builder.Services.AddTransient<IExceptionToMessageMapper, T>();

            return builder;
        }

        public static IDrexBuilder AddRabbitMq<TContext>(this IDrexBuilder builder,
            string sectionName = SectionName,
            string redisSectionName = "redis", Func<IRabbitMqPluginRegister, IRabbitMqPluginRegister> plugins = null)
            where TContext : class, new()
        {
            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            var options = builder.GetOptions<RabbitMqOptions>(sectionName);
            var redisOptions = builder.GetOptions<RedisOptions>(redisSectionName);
            return builder.AddRabbitMq<TContext>(options, plugins, b => b.AddRedis(redisOptions));
        }

        public static IDrexBuilder AddRabbitMq<TContext>(this IDrexBuilder builder,
            Func<IRabbitMqOptionsBuilder, IRabbitMqOptionsBuilder> buildOptions,
            Func<IRabbitMqPluginRegister, IRabbitMqPluginRegister> plugins = null,
            Func<IRedisOptionsBuilder, IRedisOptionsBuilder> buildRedisOptions = null)
            where TContext : class, new()
        {
            var options = buildOptions(new RabbitMqOptionsBuilder()).Build();
            return buildRedisOptions is null
                ? builder.AddRabbitMq<TContext>(options, plugins)
                : builder.AddRabbitMq<TContext>(options, plugins, b => b.AddRedis(buildRedisOptions));
        }

        public static IDrexBuilder AddRabbitMq<TContext>(this IDrexBuilder builder, RabbitMqOptions options,
            Func<IRabbitMqPluginRegister, IRabbitMqPluginRegister> plugins = null,
            RedisOptions redisOptions = null)
            where TContext : class, new()
        {
            return builder.AddRabbitMq<TContext>(options, plugins, b => b.AddRedis(redisOptions ?? new RedisOptions()));
        }

        private static IDrexBuilder AddRabbitMq<TContext>(this IDrexBuilder builder,
            RabbitMqOptions options,
            Func<IRabbitMqPluginRegister, IRabbitMqPluginRegister> plugins, Action<IDrexBuilder> registerRedis)
            where TContext : class, new()
        {
            builder.Services.AddSingleton(options);
            builder.Services.AddSingleton<RawRabbitConfiguration>(options);
            if (!builder.TryRegister(RegistryName)) return builder;

            builder.Services.AddTransient<IBusPublisher, BusPublisher>();
            if (options.MessageProcessor?.Enabled == true)
                switch (options.MessageProcessor.Type?.ToLowerInvariant())
                {
                    case "redis":
                        registerRedis(builder);
                        builder.Services.AddTransient<IMessageProcessor, RedisMessageProcessor>();
                        break;
                    default:
                        builder.Services.AddTransient<IMessageProcessor, InMemoryMessageProcessor>();
                        break;
                }
            else
                builder.Services.AddSingleton<IMessageProcessor, EmptyMessageProcessor>();

            builder.Services.AddSingleton<ICorrelationContextAccessor>(new CorrelationContextAccessor());

            ConfigureBus<TContext>(builder, plugins);

            return builder;
        }

        private static void ConfigureBus<TContext>(IDrexBuilder builder,
            Func<IRabbitMqPluginRegister, IRabbitMqPluginRegister> plugins = null)
            where TContext : class, new()
        {
            builder.Services.AddSingleton<IInstanceFactory>(serviceProvider =>
            {
                var register = plugins?.Invoke(new RabbitMqPluginRegister(serviceProvider));
                var options = serviceProvider.GetService<RabbitMqOptions>();
                var configuration = serviceProvider.GetService<RawRabbitConfiguration>();
                var namingConventions = new CustomNamingConventions(options.Namespace);

                return RawRabbitFactory.CreateInstanceFactory(new RawRabbitOptions
                {
                    DependencyInjection = ioc =>
                    {
                        register?.Register(ioc);
                        ioc.AddSingleton(options);
                        ioc.AddSingleton(configuration);
                        ioc.AddSingleton(serviceProvider);
                        ioc.AddSingleton<INamingConventions>(namingConventions);
                    },
                    Plugins = p =>
                    {
                        register?.Register(p);
                        p.UseAttributeRouting()
                            .UseRetryLater()
                            .UseMessageContext<TContext>()
                            .UseContextForwarding();

                        if (options.MessageProcessor?.Enabled == true) p.ProcessUniqueMessages();
                    }
                });
            });

            builder.Services.AddTransient(serviceProvider => serviceProvider.GetService<IInstanceFactory>().Create());
        }

        private static IClientBuilder ProcessUniqueMessages(this IClientBuilder clientBuilder)
        {
            clientBuilder.Register(c => c.Use<ProcessUniqueMessagesMiddleware>());

            return clientBuilder;
        }

        private class CustomNamingConventions : NamingConventions
        {
            public CustomNamingConventions(string defaultNamespace)
            {
                var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
                ExchangeNamingConvention = type => GetExchange(type, defaultNamespace);
                RoutingKeyConvention = type => GetRoutingKey(type, defaultNamespace);
                QueueNamingConvention = type => GetQueueName(assemblyName, type, defaultNamespace);
                ErrorExchangeNamingConvention = () => $"{defaultNamespace}.error";
                RetryLaterExchangeConvention = span => $"{defaultNamespace}.retry";
                RetryLaterQueueNameConvetion = (exchange, span) =>
                    $"{defaultNamespace}.retry_for_{exchange.Replace(".", "_")}_in_{span.TotalMilliseconds}_ms"
                        .ToLowerInvariant();
            }

            private static string GetExchange(Type type, string defaultNamespace)
            {
                var (@namespace, key) = GetNamespaceAndKey(type, defaultNamespace);

                return (string.IsNullOrWhiteSpace(@namespace) ? key : $"{@namespace}").ToLowerInvariant();
            }

            private static string GetRoutingKey(Type type, string defaultNamespace)
            {
                var (@namespace, key) = GetNamespaceAndKey(type, defaultNamespace);
                var separatedNamespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

                return $"{separatedNamespace}{key}".ToLowerInvariant();
            }

            private static string GetQueueName(string assemblyName, Type type, string defaultNamespace)
            {
                var (@namespace, key) = GetNamespaceAndKey(type, defaultNamespace);
                var separatedNamespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

                return $"{assemblyName}/{separatedNamespace}{key}".ToLowerInvariant();
            }

            private static (string @namespace, string key) GetNamespaceAndKey(Type type, string defaultNamespace)
            {
                var attribute = type.GetCustomAttribute<MessageAttribute>();
                var @namespace = attribute?.Exchange ?? defaultNamespace;
                var key = string.IsNullOrWhiteSpace(attribute?.RoutingKey)
                    ? type.Name.Underscore()
                    : attribute.RoutingKey;

                return (@namespace, key);
            }
        }

        private class ProcessUniqueMessagesMiddleware : StagedMiddleware
        {
            private readonly ILogger<ProcessUniqueMessagesMiddleware> _logger;
            private readonly IServiceProvider _serviceProvider;

            public ProcessUniqueMessagesMiddleware(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
                _logger = serviceProvider.GetRequiredService<ILogger<ProcessUniqueMessagesMiddleware>>();
            }

            public override string StageMarker { get; } = global::RawRabbit.Pipe.StageMarker.MessageDeserialized;

            public override async Task InvokeAsync(IPipeContext context,
                CancellationToken token = new CancellationToken())
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var messageProcessor = scope.ServiceProvider.GetRequiredService<IMessageProcessor>();
                    var messageId = context.GetDeliveryEventArgs().BasicProperties.MessageId;
                    _logger.LogTrace($"Received a unique message with id: {messageId} to be processed.");
                    if (!await messageProcessor.TryProcessAsync(messageId))
                    {
                        _logger.LogTrace($"A unique message with id: {messageId} was already processed.");
                        return;
                    }

                    try
                    {
                        _logger.LogTrace($"Processing a unique message with id: {messageId}...");
                        await Next.InvokeAsync(context, token);
                        _logger.LogTrace($"Processed a unique message with id: {messageId}.");
                    }
                    catch
                    {
                        _logger.LogTrace($"There was an error when processing a unique message with id: {messageId}.");
                        await messageProcessor.RemoveAsync(messageId);
                        throw;
                    }
                }
            }
        }

        private class EmptyMessageProcessor : IMessageProcessor
        {
            public Task<bool> TryProcessAsync(string id)
            {
                return Task.FromResult(true);
            }

            public Task RemoveAsync(string id)
            {
                return Task.CompletedTask;
            }
        }
    }
}