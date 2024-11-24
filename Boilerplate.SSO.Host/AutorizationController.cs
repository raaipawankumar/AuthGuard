using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Boilerplate.SSO.Host
{
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        [HttpPost("connect/token")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest()
                 ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsClientCredentialsGrantType())
            {
                return SignInForClientCredentials(request);
            }

            if (request.IsAuthorizationCodeGrantType())
            {

               return await SignInForAuthorizationCode();
            }
            throw new NotImplementedException("The specified grant type is not implemented.");
        }
        private SignInResult SignInForClientCredentials(OpenIddictRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ClientId)) throw new InvalidOperationException("The client_id parameter cannot be null or empty.");
            var claimsIdentity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            claimsIdentity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId);
            claimsIdentity.AddClaim("some-claim", "some-value", OpenIddictConstants.Destinations.AccessToken);
            var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);
            claimsPrinciple.SetScopes(request.GetScopes());
            return SignIn(claimsPrinciple, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        private async Task<SignInResult> SignInForAuthorizationCode()
        {
            var authResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            return SignIn(authResult.Principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        [HttpGet("connect/authorize")]
        [HttpPost("connect/authorize")]
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
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
                );
            }
            var claims = new List<Claim>
            {
                new(OpenIddictConstants.Claims.Subject, authResult.Principal.Identity.Name ?? "anonymous"),
                new Claim(OpenIddictConstants.Claims.Role, "public-user").SetDestinations(OpenIddictConstants.Destinations.AccessToken),
                new Claim(OpenIddictConstants.Claims.Email, "pawankumarraai@gmail.com").SetDestinations(OpenIddictConstants.Destinations.IdentityToken)
            };
            var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            claimsPrincipal.SetScopes(request.GetScopes());
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }


    }
}
