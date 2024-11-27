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
    public static async Task<SignInResult> SignIn(HttpContext context,
        UserManager<ApplicationUser> userManager,
        string username, string password, bool rememberMe = false)
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
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName ?? string.Empty));
        identity.AddClaim(new Claim(ClaimTypes.Email, user.Email ?? string.Empty));
        identity.AddClaim(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty));
        identity.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", roles)));
        identity.AddClaim(new Claim(ClaimConstants.FirstName, user.FirstName));
        identity.AddClaim(new Claim(ClaimConstants.FirstName, user.MiddleName ?? string.Empty));
        identity.AddClaim(new Claim(ClaimConstants.FirstName, user.LastName));
       
        var principal = new ClaimsPrincipal(identity);
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = rememberMe });
       
        return new SignInResult();
    }
    public static async Task SignOut(HttpContext context)
    {
        
         await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
           {
               RedirectUri = PageConstants.LoggedOut,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(-1),
           });
    }
}
