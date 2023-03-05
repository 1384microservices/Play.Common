using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Configuration;
using Play.Common.Settings;

namespace Play.Common.MongoDB;

public static class Extensions
{
    public static IServiceCollection AddMongo(this IServiceCollection serviceCollection)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));

        serviceCollection.AddSingleton(serviceProvider =>
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var mongoClient = new MongoClient(configuration.GetSection<MongoDbSettings>().ConnectionString);
            var database = mongoClient.GetDatabase(configuration.GetSection<ServiceSettings>().Name);
            return database;
        });

        return serviceCollection;
    }

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection serviceCollection, string collectionName) where T : IEntity
    {
        serviceCollection.AddSingleton<IRepository<T>>(svc =>
        {
            var database = svc.GetService<IMongoDatabase>();
            var repository = new MongoRepository<T>(database, collectionName);
            return repository;
        });

        return serviceCollection;
    }
}