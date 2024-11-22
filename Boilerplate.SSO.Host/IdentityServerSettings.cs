using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Boilerplate.SSO.Web;

public static class IdentityServerSettings
{
    public static List<TestUser> TestUsers
    {
        get
        {
            return [new TestUser { Username = "Pawan", SubjectId = "Subject1", IsActive = true }];
        }
    }
}
