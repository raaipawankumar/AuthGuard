using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthGuard.Host.Pages.Grants
{
    public class IndexModel : PageModel
    {
        public GrantsViewModel View { get; set; } = new ();
        public void OnGet()
        {
        }
    }
}
