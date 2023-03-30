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
        return services.AddMassTransitWithRabbitMQ(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }

    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services, Action<IRetryConfigurator> retryConfigurator)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumers(Assembly.GetEntryAssembly());
            busConfigurator.UsingRabbitMq((ctx, cfg) =>
            {
                var configuration = ctx.GetService<IConfiguration>();
                var rabbitMqConfiguration = configuration.GetRabbitMQSettings();
                var serviceSettings = configuration.GetServiceSettings();
                cfg.Host(rabbitMqConfiguration.Host);
                cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(serviceSettings.Name, false));
                cfg.UseMessageRetry(retryConfigurator);
            });
        });
        services.AddMassTransitHostedService();
        return services;
    }

    // private static void UsingPlayEconomyRabbitMQ(this IServiceCollectionBusConfigurator serviceCollectionBusConfigurator, Action<IRetryConfigurator> retryConfigurator)
    // {
    //     serviceCollectionBusConfigurator.UsingRabbitMq((ctx, cfg) =>
    //     {
    //         var configuration = ctx.GetService<IConfiguration>();
    //         var rabbitMqConfiguration = configuration.GetRabbitMQSettings();
    //         var serviceSettings = configuration.GetServiceSettings();
    //         cfg.Host(rabbitMqConfiguration.Host);
    //         cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(serviceSettings.Name, false));
    //         cfg.UseMessageRetry(retryConfigurator);
    //     });
    // }

    // private static void UsingPlayEconomyRabbitMQ(this IServiceCollectionBusConfigurator serviceCollectionBusConfigurator)
    // {

    //     serviceCollectionBusConfigurator.UsingPlayEconomyRabbitMQ(retryConfigurator =>
    //     {
    //         retryConfigurator.Interval(3, TimeSpan.FromSeconds(3));
    //     });

    //     serviceCollectionBusConfigurator.UsingRabbitMq((ctx, cfg) =>
    //     {
    //         var configuration = ctx.GetService<IConfiguration>();
    //         cfg.Host(configuration.GetRabbitMQSettings().Host);
    //         cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(configuration.GetServiceSettings().Name, false));
    //         cfg.UseMessageRetry(retryCfg =>
    //         {
    //             retryCfg.Interval(3, TimeSpan.FromSeconds(3));
    //         });
    //     });
    // }
}
