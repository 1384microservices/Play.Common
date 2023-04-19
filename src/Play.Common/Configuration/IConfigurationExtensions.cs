using Microsoft.Extensions.Configuration;
using Play.Common.Settings;

namespace Play.Common.Configuration;

public static class Extensions
{
    public static ServiceBusSettings GetServiceBusSettings(this IConfiguration configuration)
    {
        return configuration.GetSection<ServiceBusSettings>();
    }

    public static RabbitMQSettings GetRabbitMQSettings(this IConfiguration configuration)
    {
        return configuration.GetSection<RabbitMQSettings>();
    }

    public static ServiceSettings GetServiceSettings(this IConfiguration configuration)
    {
        return configuration.GetSection<ServiceSettings>();
    }

    public static MongoDbSettings GetMongoDbSettings(this IConfiguration configuration)
    {
        return configuration.GetSection<MongoDbSettings>();
    }

    public static T GetSection<T>(this IConfiguration configuration)
    {
        return configuration.GetSection(typeof(T).Name).Get<T>();
    }
}