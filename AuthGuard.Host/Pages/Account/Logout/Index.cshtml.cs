using AuthGuard.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthGuard.Host.Pages.Account.Logout
{
     public class IndexModel () : PageModel
    {
        [BindProperty]
        public string LogoutId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public void OnGet()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var firstName = User.FindFirst(ClaimConstants.FirstName)?.Value ?? string.Empty;
                var middleName = User.FindFirst(ClaimConstants.MiddleName)?.Value ?? string.Empty;
                var lastName = User.FindFirst(ClaimConstants.LastName)?.Value ?? string.Empty;
                FullName = string.IsNullOrWhiteSpace(middleName) 
                    ? $"{firstName} {lastName}".Trim() : $"{firstName} {middleName} {lastName}".Trim();
            
            }
           
          
        }
        public async Task OnPostAsync()
        {
          await UserAuthentication.SignOut(HttpContext);
          
        }
    }
}
