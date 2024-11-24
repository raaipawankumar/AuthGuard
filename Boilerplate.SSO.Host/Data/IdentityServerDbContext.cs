using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.SSO.Host.Data;

public class IdentityServerDbContext(DbContextOptions options) : IdentityDbContext(options)
{
}
