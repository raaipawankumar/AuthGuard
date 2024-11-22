using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Boilerplate.SSO.Host.Pages.Consent
{
    public class ConsentModel : PageModel
    {
        [BindProperty]
        public ConsentViewModel View{ get; set; } = new();
        public void OnGet()
        {
        }
    }
}
