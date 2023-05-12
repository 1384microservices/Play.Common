using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Configuration;

namespace Play.Common.Logging;

public static class Extensions
{
    public static IServiceCollection AddSeqLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var seqSettings = configuration.GetSeqSettings();
        return services.AddLogging(builder => builder.AddSeq(seqSettings.Host));
    }
}