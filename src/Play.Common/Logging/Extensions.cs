using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Configuration;
using Play.Common.Settings;

namespace Play.Common.Logging;

public static class Extensions
{
    public static IServiceCollection AddSeqLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var seqSettings = configuration.GetSeqSettings();
        return services.AddLogging(builder => builder.AddSeq(serverUrl:seqSettings.Host));
    }

    public static IServiceCollection AddSeqLogging(this IServiceCollection services, SeqSettings seqSettings) {
        return services.AddLogging(builder => builder.AddSeq(serverUrl: seqSettings.Host));
    }
}