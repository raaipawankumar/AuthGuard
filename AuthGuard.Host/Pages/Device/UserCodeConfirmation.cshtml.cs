using AuthGuard.Host.Pages.Device;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthGuard.Host.Pages.Device
{
    public class UserCodeConfirmationModel : PageModel
    {
        public DeviceAuthorizationViewModel View { get; set; } = new();
        public void OnGet()
        {
        }
    }
}
