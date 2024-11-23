using Boilerplate.SSO.Host.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Boilerplate.SSO.Host.Pages.Account
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
           
            return Page();
        }
        public async Task<IActionResult> OnPostCancel(string provider, string? returnUrl)
        {
            return Redirect("~/Account/Login");
        }
    }
}
