using System.ComponentModel.DataAnnotations;
using Common.Identity;

namespace Boilerplate.SSO.Api;

public class RegisterUserRequest
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string .Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Organization { get; set; }
    public string Mobile { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public ApplicationUser ToApplicationUser()
    {
        return new ApplicationUser
        {
            UserName = UserName,
            Email = Email,
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            DateOfBirth = DateOfBirth,
            Organization = Organization,
            PhoneNumber = Mobile,
            ImageUrl = ImageUrl
        };
    }
}

