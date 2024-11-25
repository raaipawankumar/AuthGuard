using System;
using Microsoft.AspNetCore.Identity;

namespace AuthGuard.Host.Data;

public class ApplicationRole : IdentityRole
{
    public bool IsSuperAdmin { get; set; }
    public string? DefaultPageUrl { get; set; }
}
