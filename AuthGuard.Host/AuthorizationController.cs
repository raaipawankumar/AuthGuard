using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthGuard.Host
{
    [ApiController]
    [Route("connect")]
    public class AuthorizationController(IOpenIddictApplicationManager applicationManager,
     IOpenIddictScopeManager scopeManager) : ControllerBase
    {

        [HttpPost("token")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest()
                 ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsClientCredentialsGrantType())
            {
                return await SignInForClientCredentials(request);
            }

            if (request.IsAuthorizationCodeGrantType())
            {

                return await SignInForAuthorizationCode();
            }

            if (request.IsRefreshTokenGrantType())
            {
                return await SignInForRefreshToken();
            }
            throw new NotImplementedException("The specified grant type is not implemented.");
        }

        private async Task<IActionResult> SignInForRefreshToken()
        {
            var claimPrinciple = (await HttpContext.AuthenticateAsync(
                 OpenIddictServerAspNetCoreDefaults.AuthenticationScheme))
                 .Principal
                 ?? throw new InvalidOperationException("The principal cannot be null.");

            return SignIn(claimPrinciple, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<SignInResult> SignInForClientCredentials(OpenIddictRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ClientId)) throw new InvalidOperationException("The client_id parameter cannot be null or empty.");
            var application = await applicationManager.FindByClientIdAsync(request.ClientId!)
                ?? throw new InvalidOperationException("The application details cannot be found in the database.");
            var identity = new ClaimsIdentity(authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);
            identity.AddClaim(Claims.Subject, request.ClientId);
            identity.AddClaim("some-claim", "some-value", Destinations.AccessToken);
            var claimsPrinciple = new ClaimsPrincipal(identity);
            claimsPrinciple.SetScopes(request.GetScopes());
            return SignIn(claimsPrinciple, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        private async Task<SignInResult> SignInForAuthorizationCode()
        {
            var authResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            return SignIn(authResult.Principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        [HttpGet("authorize")]
        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            var authResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
            {
                return Challenge(new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(Request.Query).ToString()
                },
                    CookieAuthenticationDefaults.AuthenticationScheme
                );
            }
            var claims = new List<Claim>
            {
                new(Claims.Subject, authResult.Principal.Identity.Name ?? "anonymous"),
                new Claim(Claims.Role, "public-user").SetDestinations(Destinations.AccessToken),
                new Claim(Claims.Email, "pawankumarraai@gmail.com").SetDestinations(Destinations.IdentityToken)
            };
            var claimsIdentity = new ClaimsIdentity(claims: claims,
             authenticationType: TokenValidationParameters.DefaultAuthenticationType,
             nameType: Claims.Name, roleType: Claims.Role);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            claimsPrincipal.SetScopes(request.GetScopes());
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }


    }
}
