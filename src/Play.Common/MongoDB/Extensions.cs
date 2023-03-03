using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
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
            var serviceSettings = configuration?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = configuration?.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings?.ConnectionString);
            var database = mongoClient.GetDatabase(serviceSettings?.Name);
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