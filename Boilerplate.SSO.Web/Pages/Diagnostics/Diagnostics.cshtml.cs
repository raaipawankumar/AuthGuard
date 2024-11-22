using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Boilerplate.SSO.Host.Pages.Diagnostics
{
    public class DiagnosticsModel : PageModel
    {
        public async void OnGet()
        {
            var localAddresses = new string[] { "127.0.0.1", "::1", HttpContext.Connection.LocalIpAddress.ToString() };
            if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress.ToString()))
            {
               
            }

            var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
            
        }
    }
}
