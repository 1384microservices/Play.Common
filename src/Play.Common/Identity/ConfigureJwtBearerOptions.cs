using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Play.Common.Configuration;
using Play.Common.Settings;

namespace Play.Common.Identity;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private const string AccessTokenParameter = "access_token";
    private const string MessageHubPath = "/messageHub";
    private readonly IConfiguration _configuration;

    public ConfigureJwtBearerOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        if (name == JwtBearerDefaults.AuthenticationScheme)
        {
            var serviceSettings = _configuration.GetSection<ServiceSettings>();

            options.Authority = serviceSettings.Authority;
            options.Audience = serviceSettings.Name;
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = (context) =>
                {
                    var token = context.Request.Query[AccessTokenParameter];
                    var path = context.HttpContext.Request.Path;
                    if (string.IsNullOrEmpty(token) && path.StartsWithSegments(MessageHubPath))
                    {
                        context.Token = token;
                    }

                    return Task.CompletedTask;
                }
            };
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(Options.DefaultName, options);
    }
}
