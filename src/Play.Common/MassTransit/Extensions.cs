using GreenPipes;
using System;
using System.Reflection;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Configuration;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using GreenPipes.Configurators;

namespace Play.Common.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumers(Assembly.GetEntryAssembly());
            busConfigurator.UsingPlayEconomyRabbitMQ(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
        });
        services.AddMassTransitHostedService();
        return services;
    }

    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services, Action<IRetryConfigurator> retryConfigurator)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumers(Assembly.GetEntryAssembly());
            busConfigurator.UsingPlayEconomyRabbitMQ(retryConfigurator);
        });
        services.AddMassTransitHostedService();
        return services;
    }

    public static void UsingPlayEconomyRabbitMQ(this IServiceCollectionBusConfigurator serviceCollectionBusConfigurator)
    {

        serviceCollectionBusConfigurator.UsingPlayEconomyRabbitMQ(retryConfigurator => retryConfigurator.Interval(3, TimeSpan.FromSeconds(3)));
    }

    public static void UsingPlayEconomyRabbitMQ(this IServiceCollectionBusConfigurator serviceCollectionBusConfigurator, Action<IRetryConfigurator> retryConfigurator)
    {
        serviceCollectionBusConfigurator.UsingRabbitMq((ctx, cfg) =>
        {
            var configuration = ctx.GetService<IConfiguration>();
            var rabbitMqConfiguration = configuration.GetRabbitMQSettings();
            var serviceSettings = configuration.GetServiceSettings();
            cfg.Host(rabbitMqConfiguration.Host);
            cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(serviceSettings.Name, false));
            cfg.UseMessageRetry(retryConfigurator);
        });
    }
}
