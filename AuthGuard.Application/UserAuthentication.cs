using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Abstractions;


namespace AuthGuard.Application;

public static class UserAuthentication
{
    public static async Task<SignInResult> SignIn(HttpContext httpContext, UserManager<ApplicationUser> userManager, string username, string password)
   {
        var user = await userManager.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == username || u.Email == username || u.PhoneNumber == username);
          if (user == null)
            {
               return new SignInResult("Invalid username or password.");
            }

            var isValidPassword = await userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
            {
                return new SignInResult("Invalid username or password.");
            }

            var roles = await userManager.GetRolesAsync(user);
           
             var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, user.Id));
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Name, user.UserName ?? string.Empty));
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Email, user.Email ?? string.Empty));
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.PhoneNumber, user.PhoneNumber ?? string.Empty));
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Role, string.Join(",", roles)));
            identity.AddClaim(new Claim("first-name", user.FirstName));
            identity.AddClaim(new Claim("middle-name", user.MiddleName ?? string.Empty));
            identity.AddClaim(new Claim("last-name", user.LastName));
            
            var claimsPrincipal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            return new SignInResult();
    }

}
