using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Boilerplate.SSO.Host.Pages.Account
{
    public class LoggedOutModel : PageModel
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }

        public bool AutomaticRedirectAfterSignOut { get; set; }

        public string LogoutId { get; set; }
        public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;
        public string ExternalAuthenticationScheme { get; set; }

        public void OnGet()
        {
        }
    }
}