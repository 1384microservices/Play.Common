using System;
using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Configuration;


namespace Play.Common.MassTransit;

public static class Extensions
{
    private const string RabbitMQ = "RABBITMQ";
    private const string ServiceBus = "SERVICEBUS";

    #region  generic
    public static IServiceCollection AddMassTransitWithMessageBroker(this IServiceCollection services, IConfiguration configuration, Action<IRetryConfigurator> retryConfigurator = null)
    {
        var serviceSettings = configuration.GetServiceSettings();

        switch (serviceSettings.MessageBroker?.ToUpper())
        {
            case ServiceBus:
                services.AddMassTransitWithAzureServiceBus(retryConfigurator);
                break;

            case RabbitMQ:
            default:
                services.AddMassTransitWithRabbitMQ(retryConfigurator);
                break;
        }
        return services;
    }

    public static void UsingPlayEconomyMessageBroker(this IBusRegistrationConfigurator serviceCollectionBusConfigurator, IConfiguration configuration, Action<IRetryConfigurator> retryConfiguratory = null)
    {
        var serviceSettings = configuration.GetServiceSettings();
        switch (serviceSettings.MessageBroker?.ToUpper())
        {
            case ServiceBus:
                serviceCollectionBusConfigurator.UsingPlayEconomyAzureServiceBus(retryConfiguratory);
                break;

            case RabbitMQ:
            default:
                serviceCollectionBusConfigurator.UsingPlayEconomyRabbitMQ(retryConfiguratory);
                break;
        }
    }


    #endregion

    #region RabbitMQ
    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services, Action<IRetryConfigurator> retryConfigurator = null)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumers(Assembly.GetEntryAssembly());
            busConfigurator.UsingPlayEconomyRabbitMQ(retryConfigurator);
        });
        return services;
    }

    public static void UsingPlayEconomyRabbitMQ(this IBusRegistrationConfigurator serviceCollectionBusConfigurator, Action<IRetryConfigurator> retryConfigurator = null)
    {
        serviceCollectionBusConfigurator.UsingRabbitMq((ctx, cfg) =>
        {
            var configuration = ctx.GetService<IConfiguration>();
            var rabbitMqConfiguration = configuration.GetRabbitMQSettings();
            var serviceSettings = configuration.GetServiceSettings();
            cfg.Host(rabbitMqConfiguration.Host);
            cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(serviceSettings.Name, false));
            if (retryConfigurator == null)
            {
                retryConfigurator = (retry) => retry.Interval(3, TimeSpan.FromSeconds(3));
            }
            cfg.UseMessageRetry(retryConfigurator);
        });
    }
    #endregion

    #region Azure Service Bus
    public static IServiceCollection AddMassTransitWithAzureServiceBus(this IServiceCollection services, Action<IRetryConfigurator> retryConfigurator = null)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumers(Assembly.GetEntryAssembly());
            busConfigurator.UsingPlayEconomyAzureServiceBus(retryConfigurator);
        });
        return services;
    }

    public static void UsingPlayEconomyAzureServiceBus(this IBusRegistrationConfigurator serviceCollectionBusConfigurator, Action<IRetryConfigurator> retryConfigurator = null)
    {
        serviceCollectionBusConfigurator.UsingAzureServiceBus((ctx, cfg) =>
        {
            var configuration = ctx.GetService<IConfiguration>();
            var serviceBusSettings = configuration.GetServiceBusSettings();
            var serviceSettings = configuration.GetServiceSettings();
            cfg.Host(serviceBusSettings.ConnectionString);
            cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(serviceSettings.Name, false));
            if (retryConfigurator == null)
            {
                retryConfigurator = (retry) => retry.Interval(3, TimeSpan.FromSeconds(3));
            }
            cfg.UseMessageRetry(retryConfigurator);
        });
    }
    #endregion
}
