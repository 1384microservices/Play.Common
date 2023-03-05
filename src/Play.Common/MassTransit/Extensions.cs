using GreenPipes;
using System;
using System.Reflection;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Configuration;

namespace Play.Common.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumers(Assembly.GetEntryAssembly());
            cfg.UsingRabbitMq((ctx, cfg) =>
            {
                var configuration = ctx.GetService<IConfiguration>();
                cfg.Host(configuration.GetRabbitMQSettings().Host);
                cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(configuration.GetServiceSettings().Name, false));
                cfg.UseMessageRetry(retry =>
                {
                    retry.Interval(3, TimeSpan.FromSeconds(5));
                });
            });
        });

        services.AddMassTransitHostedService();

        return services;
    }
}
