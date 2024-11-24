using Boilerplate.SSO.Host.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Boilerplate.SSO.Host.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("default");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(nameof(connectionString));
        }
        services.AddDbContext<IdentityServerDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseOpenIddict();
        });

    }
    public static void AddIdentityServerDependencies(this IServiceCollection services,
     IConfiguration configuration)
    {
        services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                     .UseDbContext<IdentityServerDbContext>();
            })
            .AddServer(options =>
            {
                // Client credentials flow
                options.AllowClientCredentialsFlow();
                options.SetTokenEndpointUris("/connect/token");

                // Authorization code flow
                options.AllowAuthorizationCodeFlow();
                options.RequireProofKeyForCodeExchange();
                options.SetAuthorizationEndpointUris("/connect/authorize");
                
                options.SetUserinfoEndpointUris("/connect/userinfo");

                // Common settings
                options.AddEphemeralEncryptionKey();
                options.AddEphemeralSigningKey();
                options.DisableAccessTokenEncryption();
                options.RegisterScopes("api");

                options.AddDevelopmentEncryptionCertificate();
                options.AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .DisableTransportSecurityRequirement();
            });
       
    }
    public static void AddCookieAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });
    }
}


