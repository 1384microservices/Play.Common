using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Play.Common.Configuration;

namespace Play.Common.HealthChecks;

public static class Extensions {
    private const string MongoCheckName = "mongodb";
    private const string ReadyTagName = "Ready";
    private const string LiveTagName = "Live";

    private const string HealthEndpoint = "health";

    public static IHealthChecksBuilder AddMongoDb(this IHealthChecksBuilder builder, TimeSpan? timeout = default) {
        
        return builder.Add(new HealthCheckRegistration(
            MongoCheckName,
            serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var mongoDbSettings = configuration.GetMongoDbSettings();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                var mongoDbHealthCheck = new MongoDbHealthCheck(mongoClient);
                return mongoDbHealthCheck;
            },
            HealthStatus.Unhealthy,
            new[] { ReadyTagName },
            TimeSpan.FromSeconds(3)
        ));
    }

    public static void MapPlayEconomyHealthChecks(this IEndpointRouteBuilder endpoints) {
            endpoints.MapHealthChecks($"/{HealthEndpoint}/{ReadyTagName}", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains(ReadyTagName)
            });
            endpoints.MapHealthChecks($"/{HealthEndpoint}/{LiveTagName}", new HealthCheckOptions()
            {
                Predicate = (check) => false
            });
    }
}