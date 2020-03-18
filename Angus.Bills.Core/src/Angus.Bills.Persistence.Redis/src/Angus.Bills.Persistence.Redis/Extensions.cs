using System;
using Angus.Bills.Initializers;
using Angus.Bills.Persistence.Redis.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Angus.Bills.Persistence.Redis
{
    public static class Extensions
    {
        private const string SectionName = "redis";
        private const string RegistryName = "persistence.redis";

        public static IAngusBillsBuilder AddRedis(this IAngusBillsBuilder builder, string sectionName = SectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            var options = builder.GetOptions<RedisOptions>(sectionName);
            return builder.AddRedis(options);
        }

        public static IAngusBillsBuilder AddRedis(this IAngusBillsBuilder builder,
            Func<IRedisOptionsBuilder, IRedisOptionsBuilder> buildOptions)
        {
            var options = buildOptions(new RedisOptionsBuilder()).Build();
            return builder.AddRedis(options);
        }

        public static IAngusBillsBuilder AddRedis(this IAngusBillsBuilder builder, RedisOptions options)
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