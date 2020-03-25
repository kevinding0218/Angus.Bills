using System;
using Drex.Initializers;
using Drex.Persistence.Redis.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Drex.Persistence.Redis
{
    public static class Extensions
    {
        private const string SectionName = "redis";
        private const string RegistryName = "persistence.redis";

        public static IDrexBuilder AddRedis(this IDrexBuilder builder, string sectionName = SectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            var options = builder.GetOptions<RedisOptions>(sectionName);
            return builder.AddRedis(options);
        }

        public static IDrexBuilder AddRedis(this IDrexBuilder builder,
            Func<IRedisOptionsBuilder, IRedisOptionsBuilder> buildOptions)
        {
            var options = buildOptions(new RedisOptionsBuilder()).Build();
            return builder.AddRedis(options);
        }

        public static IDrexBuilder AddRedis(this IDrexBuilder builder, RedisOptions options)
        {
            if (!builder.TryRegister(RegistryName)) return builder;

            builder.Services.AddStackExchangeRedisCache(o =>
            {
                o.Configuration = options.ConnectionString;
                o.InstanceName = options.Instance;
            });

            return builder;
        }
    }
}