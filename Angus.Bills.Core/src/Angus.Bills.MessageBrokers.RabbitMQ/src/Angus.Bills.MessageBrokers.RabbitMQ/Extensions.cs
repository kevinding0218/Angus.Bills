using System;
using System.Linq;
using Angus.Bills.Initializers;
using Angus.Bills.MessageBrokers.RabbitMQ.Clients;
using Angus.Bills.MessageBrokers.RabbitMQ.Contexts;
using Angus.Bills.MessageBrokers.RabbitMQ.Conventions;
using Angus.Bills.MessageBrokers.RabbitMQ.Initializers;
using Angus.Bills.MessageBrokers.RabbitMQ.Internals;
using Angus.Bills.MessageBrokers.RabbitMQ.Plugins;
using Angus.Bills.MessageBrokers.RabbitMQ.Publishers;
using Angus.Bills.MessageBrokers.RabbitMQ.Serializers;
using Angus.Bills.MessageBrokers.RabbitMQ.Subscribers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Angus.Bills.MessageBrokers.RabbitMQ
{
    public static class Extensions
    {
        private const string SectionName = "rabbitmq";
        private const string RegistryName = "messageBrokers.rabbitmq";

        public static IAngusBillsBuilder AddRabbitMq(this IAngusBillsBuilder builder, string sectionName = SectionName,
            Func<IRabbitMqPluginsRegistry, IRabbitMqPluginsRegistry> plugins = null)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            var options = builder.GetOptions<RabbitMqOptions>(sectionName);
            builder.Services.AddSingleton(options);
            if (!builder.TryRegister(RegistryName)) return builder;

            if (options.HostNames is null || !options.HostNames.Any())
                throw new ArgumentException("RabbitMQ hostnames are not specified.", nameof(options.HostNames));

            builder.Services.AddSingleton<IContextProvider, ContextProvider>();
            builder.Services.AddSingleton<ICorrelationContextAccessor>(new CorrelationContextAccessor());
            builder.Services.AddSingleton<IMessagePropertiesAccessor>(new MessagePropertiesAccessor());
            builder.Services.AddSingleton<IConventionsBuilder, ConventionsBuilder>();
            builder.Services.AddSingleton<IConventionsProvider, ConventionsProvider>();
            builder.Services.AddSingleton<IConventionsRegistry, ConventionsRegistry>();
            builder.Services.AddSingleton<IRabbitMqSerializer, NewtonsoftJsonRabbitMqSerializer>();
            builder.Services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
            builder.Services.AddSingleton<IBusPublisher, RabbitMqPublisher>();
            builder.Services.AddSingleton<IBusSubscriber, RabbitMqSubscriber>();
            builder.Services.AddTransient<RabbitMqExchangeInitializer>();
            builder.Services.AddHostedService<RabbitMqHostedService>();
            builder.AddInitializer<RabbitMqExchangeInitializer>();

            var pluginsRegistry = new RabbitMqPluginsRegistry();
            builder.Services.AddSingleton<IRabbitMqPluginsRegistryAccessor>(pluginsRegistry);
            builder.Services.AddSingleton<IRabbitMqPluginsExecutor, RabbitMqPluginsExecutor>();
            plugins?.Invoke(pluginsRegistry);

            var connection = new ConnectionFactory
            {
                Port = options.Port,
                VirtualHost = options.VirtualHost,
                UserName = options.Username,
                Password = options.Password,
                RequestedConnectionTimeout = options.RequestedConnectionTimeout,
                SocketReadTimeout = options.SocketReadTimeout,
                SocketWriteTimeout = options.SocketWriteTimeout,
                RequestedChannelMax = options.RequestedChannelMax,
                RequestedFrameMax = options.RequestedFrameMax,
                RequestedHeartbeat = options.RequestedHeartbeat,
                UseBackgroundThreadsForIO = options.UseBackgroundThreadsForIO,
                DispatchConsumersAsync = true,
                Ssl = options.Ssl is null
                    ? new SslOption()
                    : new SslOption(options.Ssl.ServerName, options.Ssl.CertificatePath, options.Ssl.Enabled)
            }.CreateConnection(options.HostNames.ToList(), options.ConnectionName);

            builder.Services.AddSingleton(connection);

            ((IRabbitMqPluginsRegistryAccessor) pluginsRegistry).Get().ToList().ForEach(p =>
                builder.Services.AddTransient(p.PluginType));

            return builder;
        }

        public static IAngusBillsBuilder AddExceptionToMessageMapper<T>(this IAngusBillsBuilder builder)
            where T : class, IExceptionToMessageMapper
        {
            builder.Services.AddSingleton<IExceptionToMessageMapper, T>();

            return builder;
        }

        public static IBusSubscriber UseRabbitMq(this IApplicationBuilder app)
        {
            return new RabbitMqSubscriber(app.ApplicationServices);
        }
    }
}