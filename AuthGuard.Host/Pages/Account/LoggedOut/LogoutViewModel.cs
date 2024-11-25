using AuthGuard.Host.Pages.Account.Logout;

namespace AuthGuard.Host.Pages.Account.LoggedOut
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
