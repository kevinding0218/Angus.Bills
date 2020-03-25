using System.Collections.Generic;
using System.Reflection;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Drex.WebApi
{
    internal sealed class FormatterResolver : IJsonFormatterResolver
    {
        public static readonly IJsonFormatterResolver Instance = new FormatterResolver();

        private static readonly IJsonFormatterResolver[] Resolvers =
        {
            StandardResolver.AllowPrivateCamelCase
        };

        public static List<IJsonFormatter> Formatters { get; } = new List<IJsonFormatter>();

        public IJsonFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        private static class FormatterCache<T>
        {
            public static readonly IJsonFormatter<T> Formatter;

            static FormatterCache()
            {
                foreach (var item in Formatters)
                foreach (var implInterface in item.GetType().GetTypeInfo().ImplementedInterfaces)
                {
                    var ti = implInterface.GetTypeInfo();
                    if (ti.IsGenericType && ti.GenericTypeArguments[0] == typeof(T))
                    {
                        Formatter = (IJsonFormatter<T>) item;
                        return;
                    }
                }

                foreach (var item in Resolvers)
                {
                    var formatter = item.GetFormatter<T>();
                    if (formatter is null) continue;

                    Formatter = formatter;
                    return;
                }
            }
        }
    }
}