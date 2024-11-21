using System;
using Microsoft.AspNetCore.Identity;

namespace Common.Identity;

public class ApplicationUserValidator : IUserValidator<ApplicationUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        var errors = new List<IdentityError>();
        if (string.IsNullOrWhiteSpace(user.UserName))
        {
            errors.Add(new IdentityError
            {
                Code = "UserNameEmpty",
                Description = "Username cannot be empty."
            });

        }
        if (user.DateOfBirth.HasValue)
        {
            if (user.DateOfBirth.Value > DateTime.Now)
            {
                errors.Add(new IdentityError
                {
                    Code = "DateOfBirthInvalid",
                    Description = "Date of birth cannot be in the future."
                });
            }

            var ageInDays = DateTime.Now.Subtract(user.DateOfBirth.Value).TotalDays;
            if (ageInDays < 365)
            {
                errors.Add(new IdentityError
                {
                    Code = "DateOfBirthInvalid",
                    Description = "User must be at least one years old."
                });
            }
            
            var dateBefore100Years = DateTime.Now.AddYears(-100);
            if (dateBefore100Years.CompareTo(user.DateOfBirth.Value) < 0)
            {
                errors.Add(new IdentityError
                {
                    Code = "DateOfBirthInvalid",
                    Description = "User must be at least one years old."
                });
            }
        }




        return Task.FromResult(IdentityResult.Success);
    }
}
