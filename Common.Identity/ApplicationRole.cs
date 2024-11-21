namespace Common.Identity;
using Microsoft.AspNetCore.Identity;
public class ApplicationRole : IdentityRole<int>
{
    public string? DefaultRedirectUrl { get; set; }
}
