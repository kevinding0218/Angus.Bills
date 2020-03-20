using System.Threading.Tasks;
using Angus.Bills.CQRS.Commands;
using Angus.Bills.CQRS.Events;
using Angus.Bills.Initializers;
using Angus.Bills.MessageBrokers.CQRS.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace Angus.Bills.MessageBrokers.CQRS
{
    public static class Extensions
    {
        public static Task SendAsync<TCommand>(this IBusPublisher busPublisher, TCommand command, object messageContext)
            where TCommand : class, ICommand
        {
            return busPublisher.PublishAsync(command, messageContext: messageContext);
        }

        public static Task PublishAsync<TEvent>(this IBusPublisher busPublisher, TEvent @event, object messageContext)
            where TEvent : class, IEvent
        {
            return busPublisher.PublishAsync(@event, messageContext: messageContext);
        }

        public static IBusSubscriber SubscribeCommand<T>(this IBusSubscriber busSubscriber) where T : class, ICommand
        {
            return busSubscriber.Subscribe<T>(
                (sp, command, ctx) => sp.GetRequiredService<ICommandHandler<T>>().HandleAsync(command));
        }

        public static IBusSubscriber SubscribeEvent<T>(this IBusSubscriber busSubscriber) where T : class, IEvent
        {
            return busSubscriber.Subscribe<T>(
                (sp, @event, ctx) => sp.GetService<IEventHandler<T>>().HandleAsync(@event));
        }

        public static IAngusBillsBuilder AddServiceBusCommandDispatcher(this IAngusBillsBuilder builder)
        {
            builder.Services.AddTransient<ICommandDispatcher, ServiceBusMessageDispatcher>();
            return builder;
        }

        public static IAngusBillsBuilder AddServiceBusEventDispatcher(this IAngusBillsBuilder builder)
        {
            builder.Services.AddTransient<IEventDispatcher, ServiceBusMessageDispatcher>();
            return builder;
        }
    }
}