using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenIddict.Abstractions;

namespace Boilerplate.SSO.Host.Pages.Clients
{
    public class IndexModel(IOpenIddictApplicationManager manager) : PageModel
    {

        [BindProperty]
        [Required]
        public string ClientId { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public string ClientSecret { get; set; } = string.Empty;

        [BindProperty]
        public string? ClientType { get; set; }

        [BindProperty]
        [Required]
        public string RedirectUri { get; set; } = string.Empty;

        [BindProperty]
        public string DisplayName { get; set; } = string.Empty;

        [BindProperty]
        public string PostLogoutRedirectUri { get; set; }

        [BindProperty]
        [Required]
        public string Permissions { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                ClientType = ClientType,
                RedirectUris = { new Uri(RedirectUri) },
                DisplayName = DisplayName,
                PostLogoutRedirectUris = { new Uri(PostLogoutRedirectUri) }
            };
            if (!string.IsNullOrEmpty(Permissions))
            {
                descriptor.Permissions.UnionWith(Permissions.Split(',').Select(p => p.Trim()));
            }

            await manager.CreateAsync(descriptor);

            return RedirectToPage("/Index");
        }
    }
}
