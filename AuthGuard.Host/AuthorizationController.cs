using AuthGuard.Application;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;


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

            var principl = await ClaimsPrincipleFactory.CreateClaimsPrinciple(request, HttpContext, applicationManager);
            return SignIn(principl, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet("authorize")]
        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
            {
                return Challenge(new AuthenticationProperties
                {
                    RedirectUri = RedirectUris.BuildForChallenge(HttpContext.Request)
                },
                    CookieAuthenticationDefaults.AuthenticationScheme
                );
            }
            
            var principal = await ClaimsPrincipleFactory.CreateForAuthenticatedUser(authResult, request, scopeManager);
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return SignOut(
                  authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                  properties: new AuthenticationProperties
                  {
                      RedirectUri = "/"
                  });
        }

    }
}
