using System;
using Microsoft.AspNetCore.Identity;

namespace AuthGuard.Host.Data;

public class ApplicationRole : IdentityRole<int>
{
    public string? DefaultPageUrl { get; set; }
}
