using System.Security.Claims;
using AuthGuard.Host.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenIddict.Abstractions;

namespace AuthGuard.Host.Pages.Account.Login
{
    [AllowAnonymous]
    public class LoginModel(IConfiguration configuration) : PageModel
    {
        [BindProperty]
        public LoginInputModel Input { get; set; } = new ();
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;

        public IEnumerable<ExternalProvider> ExternalProviders => configuration.GetExternalProviders();

        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders.Count() == 1;
        public string? ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.Scheme : null;
        
        public void OnGet(string? returnUrl)
        {
            Input.ReturnUrl = returnUrl ??string.Empty;
        }
        public async Task<IActionResult> OnPostAsync(string? button)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await SignInUser();

            return RedirectUser(Input.ReturnUrl);
        }

        private async Task SignInUser()
        {
            var claimsIdentity = new ClaimsIdentity(
            [
                new Claim(OpenIddictConstants.Claims.Name,Input.Username),
                new Claim(ClaimTypes.Role, "public-user")
            ]
            , CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }

        private RedirectResult RedirectUser(string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect("~/");
            }

            if (!Url.IsLocalUrl(returnUrl))
            {
                throw new InvalidOperationException($"The return url is not local: '{returnUrl}'");
            }

            return Redirect(returnUrl);
        }
        public IActionResult OnPostCancel(string provider, string? returnUrl)
        {
            return Redirect("~/Account/Login/Index");
        }
    }
}
