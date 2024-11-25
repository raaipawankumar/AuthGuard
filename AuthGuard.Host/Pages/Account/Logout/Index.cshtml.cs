using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Boilerplate.SSO.Host.Pages.Account.Logout
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string LogoutId { get; set; } = string.Empty;
        public void OnGet()
        {
        }
        public void OnPost()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new AuthenticationProperties
            {
                RedirectUri = WebUtility.UrlDecode("LoggedOut/Index")
            });
        }
    }
}
