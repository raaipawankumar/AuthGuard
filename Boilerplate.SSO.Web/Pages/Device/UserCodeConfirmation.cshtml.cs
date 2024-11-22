using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Boilerplate.SSO.Host.Pages.Device
{
    public class UserCodeConfirmationModel : PageModel
    {
        public DeviceAuthorizationViewModel View { get; set; } = new();
        public void OnGet()
        {
        }
    }
}
