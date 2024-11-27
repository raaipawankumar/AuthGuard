using AuthGuard.Application;
using AuthGuard.Host.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthGuard.Host.Pages.Account.Login
{
    [AllowAnonymous]
    public class IndexModel(UserManager<ApplicationUser> userManager,
      IConfiguration configuration) : PageModel
    {
        [BindProperty]
        public LoginInputModel Input { get; set; } = new ();
       
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
            var result = await UserAuthentication.SignIn(HttpContext,
                userManager, Input.Username,
                Input.Password, Input.RememberLogin);

            if (!result.IsOk)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }
            return RedirectUser(Input.ReturnUrl);
        }
        private IActionResult RedirectUser(string? returnUrl)
        {
            returnUrl ??= Url.Content("~/LoginDetail");
            if( Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return Redirect(returnUrl);
        }
        public IActionResult OnPostCancel()
        {
            return Redirect("~/Account/Login/Index");
        }
    }
}
