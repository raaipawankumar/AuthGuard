namespace Boilerplate.SSO.Api.Extensions;

using Common.Identity;
using Microsoft.AspNetCore.Identity;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserStore<ApplicationUser>>(provider =>
        {
            var connectionString = configuration.GetConnectionString("default")!;
            return new UserStore(connectionString);
        });

        services.AddScoped<IRoleStore<ApplicationRole>>(provider =>
        {
            var connectionString = configuration.GetConnectionString("default")!;
            return new RoleStore(connectionString);
        });

        services.AddScoped<IUserValidator<ApplicationUser>, ApplicationUserValidator>();

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
   
        }

        ).AddDefaultTokenProviders();

    }
}

