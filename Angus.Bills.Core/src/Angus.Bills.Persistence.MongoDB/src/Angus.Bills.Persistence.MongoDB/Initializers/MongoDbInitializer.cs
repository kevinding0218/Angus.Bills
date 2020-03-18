using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Angus.Bills.Persistence.MongoDB.Initializers
{
    internal sealed class MongoDbInitializer : IMongoDbInitializer
    {
        private static int _initialized;
        private readonly IMongoDatabase _database;
        private readonly bool _seed;

        public MongoDbInitializer(IMongoDatabase database, MongoDbOptions options)
        {
            _database = database;
            _seed = options.Seed;
        }

        public Task InitializeAsync()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 1) return Task.CompletedTask;

            RegisterConventions();

            return Task.CompletedTask;
        }

        private void RegisterConventions()
        {
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?),
                new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
            ConventionRegistry.Register("angus-bills", new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String)
            }, _ => true);
        }
    }
}