using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Boilerplate.SSO.Host.Pages.Grants
{
    public class GrantsModel : PageModel
    {
        public GrantsViewModel View { get; set; } = new ();
        public void OnGet()
        {
        }
    }
}
