using System;

namespace Boilerplate.SSO.Host;

public static class AllowedScopes
{
    public const string OpenId = "openid";
    public const string Profile = "profile";
    public const string Email = "email";
    public const string Phone = "phone";
    public const string FirstName = "first_name";
    public const string LastName = "last_name";
    public const string Role = "role";
    public static string[] AllScopes =
    [
        OpenId,
        Profile,
        Email,
        Phone,
        FirstName,
        LastName,
        Role
    ];
}
