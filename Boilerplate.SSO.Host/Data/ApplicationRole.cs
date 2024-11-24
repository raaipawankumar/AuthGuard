using System;
using Microsoft.AspNetCore.Identity;

namespace Boilerplate.SSO.Host.Data;

public class ApplicationRole: IdentityRole<int>
{
    public string? DefaultPageUrl { get; set; }
}
