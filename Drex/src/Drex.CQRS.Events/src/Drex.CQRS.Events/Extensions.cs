using System;
using Drex.CQRS.Events.Dispatchers;
using Drex.Initializers;
using Microsoft.Extensions.DependencyInjection;

namespace Drex.CQRS.Events
{
    public static class Extensions
    {
        public static IDrexBuilder AddEventHandlers(this IDrexBuilder builder)
        {
            builder.Services.Scan(s =>
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            return builder;
        }

        public static IDrexBuilder AddInMemoryEventDispatcher(this IDrexBuilder builder)
        {
            builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
            return builder;
        }
    }
}