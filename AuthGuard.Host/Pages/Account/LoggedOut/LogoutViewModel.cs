using Boilerplate.SSO.Host.Pages.Account;

namespace AuthGuard.Host.Pages.Account.LoggedOut
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
