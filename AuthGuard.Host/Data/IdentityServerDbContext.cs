using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthGuard.Host.Data;

public class IdentityServerDbContext(DbContextOptions options) : IdentityDbContext(options)
{
}
