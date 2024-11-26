using System;
using Microsoft.AspNetCore.Identity;

namespace AuthGuard.Application;

public class ApplicationRole : IdentityRole
{
    public bool IsSuperAdmin { get; set; }
    public string? DefaultPageUrl { get; set; }
}
