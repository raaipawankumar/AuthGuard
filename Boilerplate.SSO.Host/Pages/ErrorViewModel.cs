//using IdentityServer4.Models;

namespace Boilerplate.SSO.Host.Pages
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
        }

        public ErrorViewModel(string error)
        {
            Error = error;
        }

        public string Error { get; set; }
    }
}