using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Drex.CQRS.Commands;
using Drex.CQRS.Events;
using Drex.Initializers;
using Drex.Logging.CQRS.Decorators;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Drex.Logging.CQRS
{
    public static class Extensions
    {
        public static IDrexBuilder AddCommandHandlersLogging(this IDrexBuilder builder,
            Assembly assembly = null)
        {
            return builder.AddHandlerLogging(typeof(ICommandHandler<>), typeof(CommandHandlerLoggingDecorator<>),
                assembly);
        }

        public static IDrexBuilder AddEventHandlersLogging(this IDrexBuilder builder,
            Assembly assembly = null)
        {
            return builder.AddHandlerLogging(typeof(IEventHandler<>), typeof(EventHandlerLoggingDecorator<>), assembly);
        }

        private static IDrexBuilder AddHandlerLogging(this IDrexBuilder builder, Type handlerType,
            Type decoratorType, Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();

            var handlers = assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
                .ToList();

            handlers.ForEach(ch => GetExtensionMethods()
                .FirstOrDefault(mi => !mi.IsGenericMethod && mi.Name == "Decorate")?
                .Invoke(builder.Services, new object[]
                {
                    builder.Services,
                    ch.GetInterfaces().FirstOrDefault(),
                    decoratorType.MakeGenericType(ch.GetInterfaces().FirstOrDefault()?.GenericTypeArguments.First())
                }));

            return builder;
        }

        private static IEnumerable<MethodInfo> GetExtensionMethods()
        {
            var types = typeof(ReplacementBehavior).Assembly.GetTypes();

            var query = from type in types
                where type.IsSealed && !type.IsGenericType && !type.IsNested
                from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                where method.IsDefined(typeof(ExtensionAttribute), false)
                where method.GetParameters()[0].ParameterType == typeof(IServiceCollection)
                select method;
            return query;
        }
    }
}