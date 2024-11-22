using System.Net.Http.Headers;
using Boilerplate.SSO.Host;
using Common.Identity;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

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
        services.AddTransient<IClientStore>(provider => {
        var clientStore = new ValidatingClientStore<ClientStore>(new ClientStore(connectionString),
            provider.GetRequiredService<IClientConfigurationValidator>(),
            provider.GetRequiredService<IEventService>(),
            provider.GetRequiredService<ILogger<ValidatingClientStore<ClientStore>>>());
        return new CachedClientStore(clientStore, new MemoryCache(new MemoryCacheOptions()));
        });
        services.AddTransient<IResourceStore>(provider => new ResourceStore(connectionString));
       

        services.AddIdentityServer()
        .AddSecretValidator<Host.SecretValidator>()
            .AddDeveloperSigningCredential();
    }
    public static void AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddCookie("cookie", options =>
        {
           options.Cookie.Name = "ExternalCookie";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
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


