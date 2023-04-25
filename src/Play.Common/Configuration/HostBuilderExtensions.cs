using System;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
