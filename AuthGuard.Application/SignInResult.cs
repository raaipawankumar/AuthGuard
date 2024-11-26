using System;

namespace AuthGuard.Application;

public record SignInResult(params string[] Errors)
{
    public bool IsOk => Errors.Length == 0;
}
