using System;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Play.Common.Settings;

namespace Play.Common.Configuration;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureAzureKeyVault(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((context, configurationBuilder) =>
        {
            if (context.HostingEnvironment.IsProduction())
            {
                var configuration = configurationBuilder.Build();
                var serviceSettings = configuration.GetServiceSettings();
                var uri = new Uri($"https://{serviceSettings.KeyVaultName}.vault.azure.net/");
                var credentials = new DefaultAzureCredential();
                configurationBuilder.AddAzureKeyVault(uri, credentials);
            }
        });
        return hostBuilder;
    }
}

public static class ConfigurationExtensions
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