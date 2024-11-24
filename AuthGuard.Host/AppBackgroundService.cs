using System;
using AuthGuard.Host.Data;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthGuard.Host;

public class AppBackgroundService(IServiceProvider serviceProvider) : IHostedService
{

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<IdentityServerDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        await AddClients(manager, cancellationToken);
        await AddScopes(scopeManager, cancellationToken);
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await AddUsers(userManager, cancellationToken);

    }
    private static async Task AddClients(IOpenIddictApplicationManager manager, CancellationToken cancellationToken)
    {
        if (await manager.FindByClientIdAsync("service-worker", cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "service-worker",
                ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials
                }
            }, cancellationToken);
        }
        if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "postman",
                ClientSecret = "postman-secret",
                RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.Introspection,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.GrantTypes.RefreshToken,

                }
            }, cancellationToken);
        }
    }
    private static async Task AddScopes(IOpenIddictScopeManager scopeManager, CancellationToken cancellationToken)
    {

        OpenIddictScopeDescriptor scopeDescriptor = new OpenIddictScopeDescriptor
        {
            Name = "test_scope",
            Resources = { "test_resource" }
        };

        var scope = await scopeManager.FindByNameAsync(scopeDescriptor.Name, cancellationToken);
        if (scope == null)
        {
            await scopeManager.CreateAsync(scopeDescriptor, cancellationToken);
        }

    }
    private static async Task AddUsers(UserManager<ApplicationUser> userManager, CancellationToken cancellationToken)
    {
        if (await userManager.FindByNameAsync("superadmin") is null)
        {
            var user = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "pawankumarraai@gmail.com"
            };
            await userManager.CreateAsync(user, "superadmin");
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
