using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;
using AuthGuard.Host.Data;
using AuthGuard.Host.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthGuard.Host.Pages.Account.Login
{
    [AllowAnonymous]
    public class IndexModel(UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
     IConfiguration configuration) : PageModel
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
            Input.ReturnUrl = returnUrl ?? string.Empty;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

           var user = await FindUserAsync(Input.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            var isValidPassword = await userManager.CheckPasswordAsync(user, Input.Password);
            if (!isValidPassword)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }
            var roles = await userManager.GetRolesAsync(user);
            await UpdateContextWithSignIn(user, roles);

            return RedirectUser(Input.ReturnUrl);
        }
        private async Task<ApplicationUser?> FindUserAsync(string username)
        {
        var user = await userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == username || u.Email == username || u.PhoneNumber == username);

        return user;
        }
        private async Task UpdateContextWithSignIn(ApplicationUser user, IList<string> roles)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            identity.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", roles)));
            identity.AddClaim(new Claim("first-name", user.FirstName, ClaimValueTypes.String, Destinations.IdentityToken));
            identity.AddClaim(new Claim("middle-name", user.MiddleName, ClaimValueTypes.String, Destinations.IdentityToken));
            identity.AddClaim(new Claim("last-name", user.LastName, ClaimValueTypes.String, Destinations.IdentityToken));
            
            var claimsPrincipal = new ClaimsPrincipal(identity);
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
