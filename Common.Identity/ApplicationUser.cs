using Microsoft.AspNetCore.Identity;

namespace Common.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Organization { get; set; }
    public string? ImageUrl { get; set; }
}
