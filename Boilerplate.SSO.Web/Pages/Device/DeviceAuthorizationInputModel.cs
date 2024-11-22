using Boilerplate.SSO.Host.Pages.Consent;

namespace Boilerplate.SSO.Host.Pages.Device;

public class DeviceAuthorizationInputModel : ConsentInputModel
{
    public string UserCode { get; set; }
}