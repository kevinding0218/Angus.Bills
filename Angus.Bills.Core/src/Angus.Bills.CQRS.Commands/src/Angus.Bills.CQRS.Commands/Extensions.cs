using System;
using Angus.Bills.CQRS.Commands.Dispatchers;
using Angus.Bills.Initializers;
using Microsoft.Extensions.DependencyInjection;

namespace Angus.Bills.CQRS.Commands
{
    public static class Extensions
    {
        public static IAngusBillsBuilder AddCommandHandlers(this IAngusBillsBuilder builder)
        {
            builder.Services.Scan(s =>
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            return builder;
        }

        public static IAngusBillsBuilder AddInMemoryCommandDispatcher(this IAngusBillsBuilder builder)
        {
            builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            return builder;
        }
    }
}