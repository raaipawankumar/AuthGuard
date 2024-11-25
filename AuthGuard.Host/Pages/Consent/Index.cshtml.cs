using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthGuard.Host.Pages.Consent
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ConsentViewModel View { get; set; } = new();
        public void OnGet()
        {
        }
    }
}
