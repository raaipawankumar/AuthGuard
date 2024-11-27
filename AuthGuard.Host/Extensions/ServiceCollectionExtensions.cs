using AuthGuard.Application;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Server;


namespace AuthGuard.Host.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
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
        return services;
    }
    public static IServiceCollection AddIdentityDependencies(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<IdentityServerDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
    public static IServiceCollection AddIdentityServerDependencies(this IServiceCollection services,
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
                options.SetFlows();
                options.SetEndpoints();
                options.RegisterScopes(AllowedScopes.AllScopes);
                options.AddEphemeralEncryptionKey();
                options.AddEphemeralSigningKey();
                options.DisableAccessTokenEncryption();
                
                options.AddDevelopmentEncryptionCertificate();
                options.AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .DisableTransportSecurityRequirement();
            });
            return services;

    }
    private static void SetFlows(this OpenIddictServerBuilder options)
    {
        options.AllowClientCredentialsFlow();
        options.AllowAuthorizationCodeFlow()
        .RequireProofKeyForCodeExchange();
        options.AllowRefreshTokenFlow();
    }
    private static void SetEndpoints(this OpenIddictServerBuilder options)
    {
        options.SetTokenEndpointUris("/connect/token");
        options.SetAuthorizationEndpointUris("/connect/authorize");
        options.SetLogoutEndpointUris("/connect/logout");
        options.SetUserinfoEndpointUris("/connect/userinfo");
        options.SetIntrospectionEndpointUris("/connect/introspect");
        options.SetRevocationEndpointUris("/connect/revoke");
    }
    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });
        return services;
    }
}


