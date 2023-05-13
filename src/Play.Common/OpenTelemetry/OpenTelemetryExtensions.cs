using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Play.Common.Settings;

namespace Play.Common.OpenTelemetry;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddTracing(this IServiceCollection services, ServiceSettings serviceSettings, JaegerSettings jaegerSettings)
    {

        services
            .AddOpenTelemetryTracing(builder =>
            {
                builder
                    .AddSource(serviceSettings.Name)
                    .AddSource("MassTransit")
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: serviceSettings.Name))
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter()
                    .AddJaegerExporter(opt =>
                    {
                        opt.AgentHost = jaegerSettings.Host;
                        opt.AgentPort = jaegerSettings.Port;
                    })
                    ;
            });


        return services;
    }
}