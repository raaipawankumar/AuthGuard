using AuthGuard.Host.Pages.Consent;

namespace AuthGuard.Host.Pages.Device;

public class DeviceAuthorizationInputModel : ConsentInputModel
{
    public string UserCode { get; set; }
}