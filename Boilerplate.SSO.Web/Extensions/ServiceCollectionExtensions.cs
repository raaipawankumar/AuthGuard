using Common.Identity;
using Microsoft.AspNetCore.Identity;

namespace Boilerplate.SSO.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserStore<ApplicationUser>>(provider =>
        {
            var connectionString = configuration.GetConnectionString("default")!;
            return new UserStore(connectionString);  
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddUserStore<UserStore>()
            .AddRoleStore<RoleStore>()
            .AddDefaultTokenProviders();
    }
}
