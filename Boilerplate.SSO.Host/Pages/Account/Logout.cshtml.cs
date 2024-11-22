using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Boilerplate.SSO.Host.Pages
{
    public class LogoutModel : PageModel
    {
        [BindProperty]
        public string LogoutId { get; set; } = string.Empty;
        public void OnGet()
        {
        }
    }
}
