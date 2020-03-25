using System;
using Drex.CQRS.Commands.Dispatchers;
using Drex.Initializers;
using Microsoft.Extensions.DependencyInjection;

namespace Drex.CQRS.Commands
{
    public static class Extensions
    {
        public static IDrexBuilder AddCommandHandlers(this IDrexBuilder builder)
        {
            builder.Services.Scan(s =>
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            return builder;
        }

        public static IDrexBuilder AddInMemoryCommandDispatcher(this IDrexBuilder builder)
        {
            builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            return builder;
        }
    }
}