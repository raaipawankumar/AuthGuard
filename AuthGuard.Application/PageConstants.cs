using System;

namespace AuthGuard.Application;

public static class PageConstants
{
    public const string Account = "/Account";
    public const string Login = $"{Account}/Login/Index";
    public const string Logout = $"{Account}/Logout/index";
    public const string Register = $"{Account}/Register/Index";

    public const string LoggedOut = $"{Account}/Loggedout/Index";


}
