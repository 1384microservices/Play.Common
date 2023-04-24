using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Identity;

public static class Extensions
{
    public static AuthenticationBuilder AddJwtBearerAuthentication(this IServiceCollection services)
    {
        return services
            .ConfigureOptions<ConfigureJwtBearerOptions>()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddCookie(opt =>
            {
                opt.CookieManager = new ChunkingCookieManager();
                opt.Cookie.HttpOnly = true;
                opt.Cookie.SameSite = SameSiteMode.None;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddJwtBearer();
    }
}