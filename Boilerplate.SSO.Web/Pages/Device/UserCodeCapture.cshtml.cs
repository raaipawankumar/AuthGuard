using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Boilerplate.SSO.Host.Pages.Device
{
    public class UserCodeCaptureModel : PageModel
    {
        [BindProperty]
        public string UserCode { get; set; }
        public void OnGet()
        {
        }
    }
}
