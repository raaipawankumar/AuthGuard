using System.Net;
using System.Security.Claims;
using AuthGuard.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthGuard.Host.Pages
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class LoginDetailModel : PageModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public void OnGet()
        {
            Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserName = User.FindFirstValue(ClaimTypes.Name);
            Email = User.FindFirstValue(ClaimTypes.Email);
            PhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
            Role = User.FindFirstValue(ClaimTypes.Role);
            FirstName = User.FindFirstValue(ClaimConstants.FirstName);
            MiddleName = User.FindFirstValue(ClaimConstants.MiddleName);
            LastName = User.FindFirstValue(ClaimConstants.LastName);

            
           
        }
    }
}
