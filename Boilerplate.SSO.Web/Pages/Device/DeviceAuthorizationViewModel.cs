using Boilerplate.SSO.Host.Pages.Consent;
namespace Boilerplate.SSO.Host.Pages.Device
{
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        public string UserCode { get; set; }
        public bool ConfirmUserCode { get; set; }
    }
}