using Microsoft.EntityFrameworkCore;

namespace Boilerplate.SSO.Host.Data;

public class IdentityServerDbContext(DbContextOptions options) : DbContext(options)
{
}
