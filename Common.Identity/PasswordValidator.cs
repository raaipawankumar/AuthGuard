using System;
using Microsoft.AspNetCore.Identity;

namespace Common.Identity;

public class PasswordValidator : IPasswordValidator<ApplicationUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string? password)
    {
       return Task.FromResult(IdentityResult.Success);
    }
}
