using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthGuard.Application;

public class IdentityServerDbContext(DbContextOptions options) :
 IdentityDbContext(options)
{
    public DbSet<ApplicationUser> Users { get; set; } 
    public DbSet<ApplicationRole> Roles { get; set; }
}
