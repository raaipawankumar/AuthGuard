using Common.Identity;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;

namespace Boilerplate.SSO.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("default");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }
        services.AddScoped<IClientStore>(provider => new ClientStore(connectionString));
        services.AddScoped<IResourceStore>(provider => new ResourceStore(connectionString));

        services.AddIdentityServer()
        .AddClientStore<ClientStore>()
        .AddResourceStore<ResourceStore>()
        .AddClientStoreCache<CachedClientStore>()
        .AddDeveloperSigningCredential();

    }
    public static void AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddCookie(IdentityConstants.ApplicationScheme, options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        })
        .AddOpenIdConnect(IdentityConstants.ExternalScheme, options =>
            {
                options.Authority = configuration["IdentityServer:Authority"];
                options.ClientId = configuration["IdentityServer:ClientId"];
                options.ClientSecret = configuration["IdentityServer:ClientSecret"];
                options.ResponseType = "code";
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

            }
        );
    }
}


