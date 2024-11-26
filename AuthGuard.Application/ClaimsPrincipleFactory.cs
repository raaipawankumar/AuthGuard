using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.AspNetCore.Authentication;

namespace AuthGuard.Application;

public static class ClaimsPrincipleFactory
{
    public static async Task<ClaimsPrincipal> CreateClaimsPrinciple(OpenIddictRequest request,
     HttpContext httpContext, IOpenIddictApplicationManager applicationManager)
    {
        if (request.IsClientCredentialsGrantType())
        {
            return await CreateForClientCredentials(request, applicationManager);
        }

        if (request.IsAuthorizationCodeGrantType())
        {
            return await CreateForAuthorizationCode(httpContext);
        }

        if (request.IsRefreshTokenGrantType())
        {
            return await CreateForRefreshToken(httpContext);
        }

        throw new NotImplementedException("The specified grant type is not implemented.");
    }
    private static async Task<ClaimsPrincipal> CreateForClientCredentials(OpenIddictRequest request,
     IOpenIddictApplicationManager applicationManager)
    {
        if (string.IsNullOrWhiteSpace(request.ClientId))
        {
            throw new InvalidOperationException("The client_id parameter cannot be null or empty.");
        }

        var application = await applicationManager.FindByClientIdAsync(request.ClientId!)
            ?? throw new InvalidOperationException($"Invalid client id: {request.ClientId}");

        var identity = new ClaimsIdentity(authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: Claims.Name,
            roleType: Claims.Role);

        identity.AddClaim(Claims.Subject, request.ClientId);
        identity.AddClaim("some-claim", "some-value", Destinations.AccessToken);

        var claimsPrinciple = new ClaimsPrincipal(identity);
        claimsPrinciple.SetScopes(request.GetScopes());
        return claimsPrinciple;
    }

    private static async Task<ClaimsPrincipal> CreateForAuthorizationCode(HttpContext httpContext)
    {
        var authResult = await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        if (authResult?.Principal == null)
        {
            throw new InvalidOperationException("The principal cannot be null for an authenticated user.");
        }

        return authResult.Principal;
    }

    private static async Task<ClaimsPrincipal> CreateForRefreshToken(HttpContext httpContext)
    {
        var result = await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        return result.Principal ?? throw new InvalidOperationException("The principal cannot be null.");
    }
    public static async Task<ClaimsPrincipal> CreateForAuthenticatedUser(
        AuthenticateResult authResult,
        OpenIddictRequest request,
        IOpenIddictScopeManager scopeManager)
    {
        if (!authResult.Succeeded) throw new InvalidOperationException("The user is not authenticated");

        var identity = new ClaimsIdentity(
         authenticationType: TokenValidationParameters.DefaultAuthenticationType,
         nameType: Claims.Name,
         roleType: Claims.Role);

        identity.AddClaims(authResult.Principal.Claims);

        var resources = new List<string>();
        await foreach (var resource in scopeManager.ListResourcesAsync(identity.GetScopes()))
        {
            resources.Add(resource);
        }

        identity.SetResources(resources);

        //  identity.SetDestinations(c => AuthorizationService.GetDestinations(identity, c));

        var principal = new ClaimsPrincipal(identity);
        principal.SetScopes(request.GetScopes());

        return principal;
    }
}



