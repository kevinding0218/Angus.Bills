using System;
using Drex.Initializers;
using Drex.Persistence.MongoDB.Builders;
using Drex.Persistence.MongoDB.Factories;
using Drex.Persistence.MongoDB.Initializers;
using Drex.Persistence.MongoDB.Repository;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Drex.Persistence.MongoDB
{
    public static class Extensions
    {
        private const string SectionName = "mongo";
        private const string RegistryName = "persistence.mongoDb";

        public static IDrexBuilder AddMongo(this IDrexBuilder builder, string sectionName = SectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            var mongoOptions = builder.GetOptions<MongoDbOptions>(sectionName);
            return builder.AddMongo(mongoOptions);
        }

        public static IDrexBuilder AddMongo(this IDrexBuilder builder, Func<IMongoDbOptionsBuilder,
            IMongoDbOptionsBuilder> buildOptions)
        {
            var mongoOptions = buildOptions(new MongoDbOptionsBuilder()).Build();
            return builder.AddMongo(mongoOptions);
        }

        public static IDrexBuilder AddMongo(this IDrexBuilder builder, MongoDbOptions mongoOptions)
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

        public static IDrexBuilder AddMongoRepository<TEntity, TIdentifiable>(this IDrexBuilder builder,
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