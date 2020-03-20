using System;
using Angus.Bills.CQRS.Events.Dispatchers;
using Angus.Bills.Initializers;
using Microsoft.Extensions.DependencyInjection;

namespace Angus.Bills.CQRS.Events
{
    public static class Extensions
    {
        public static IAngusBillsBuilder AddEventHandlers(this IAngusBillsBuilder builder)
        {
            builder.Services.Scan(s =>
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            return builder;
        }

        public static IAngusBillsBuilder AddInMemoryEventDispatcher(this IAngusBillsBuilder builder)
        {
            builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
            return builder;
        }
    }
}