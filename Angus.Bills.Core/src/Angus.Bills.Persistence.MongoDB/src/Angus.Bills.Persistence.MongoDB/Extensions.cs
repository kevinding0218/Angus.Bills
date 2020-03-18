using System;
using Angus.Bills.Initializers;
using Angus.Bills.Persistence.MongoDB.Builders;
using Angus.Bills.Persistence.MongoDB.Factories;
using Angus.Bills.Persistence.MongoDB.Initializers;
using Angus.Bills.Persistence.MongoDB.Repository;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Angus.Bills.Persistence.MongoDB
{
    public static class Extensions
    {
        private const string SectionName = "mongo";
        private const string RegistryName = "persistence.mongoDb";

        public static IAngusBillsBuilder AddMongo(this IAngusBillsBuilder builder, string sectionName = SectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            var mongoOptions = builder.GetOptions<MongoDbOptions>(sectionName);
            return builder.AddMongo(mongoOptions);
        }

        public static IAngusBillsBuilder AddMongo(this IAngusBillsBuilder builder, Func<IMongoDbOptionsBuilder,
            IMongoDbOptionsBuilder> buildOptions)
        {
            var mongoOptions = buildOptions(new MongoDbOptionsBuilder()).Build();
            return builder.AddMongo(mongoOptions);
        }

        public static IAngusBillsBuilder AddMongo(this IAngusBillsBuilder builder, MongoDbOptions mongoOptions)
        {
            if (!builder.TryRegister(RegistryName)) return builder;

            builder.Services.AddSingleton(mongoOptions);
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var options = sp.GetService<MongoDbOptions>();
                return new MongoClient(options.ConnectionString);
            });
            builder.Services.AddTransient(sp =>
            {
                var options = sp.GetService<MongoDbOptions>();
                var client = sp.GetService<IMongoClient>();
                return client.GetDatabase(options.Database);
            });
            builder.Services.AddTransient<IMongoDbInitializer, MongoDbInitializer>();
            builder.Services.AddTransient<IMongoSessionFactory, MongoSessionFactory>();

            builder.AddInitializer<IMongoDbInitializer>();

            return builder;
        }

        public static IAngusBillsBuilder AddMongoRepository<TEntity, TIdentifiable>(this IAngusBillsBuilder builder,
            string collectionName)
            where TEntity : IIdentifiable<TIdentifiable>
        {
            builder.Services.AddTransient<IMongoRepository<TEntity, TIdentifiable>>(sp =>
            {
                var database = sp.GetService<IMongoDatabase>();
                return new MongoRepository<TEntity, TIdentifiable>(database, collectionName);
            });

            return builder;
        }
    }
}