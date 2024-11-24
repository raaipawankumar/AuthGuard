using Boilerplate.SSO.Host.Pages.Account;

namespace AuthGuard.Host.Extensions;

public static class ConfigurationExtensions
{
    public static IEnumerable<ExternalProvider> GetExternalProviders(this IConfiguration configuration)
    {
        var externalProviders = new List<ExternalProvider>();
        var section = configuration.GetSection("ExternalProviders");
        foreach (var provider in section.GetChildren())
        {
            var providerName = provider["Name"];
            var providerScheme = provider["Scheme"];
            ArgumentException.ThrowIfNullOrWhiteSpace(providerName, nameof(providerName));
            ArgumentException.ThrowIfNullOrWhiteSpace(providerScheme, nameof(providerScheme));
            externalProviders.Add(new ExternalProvider
            {
                Name = provider["Name"],
                Scheme = provider["Scheme"]
            });
        }
        return externalProviders;

    }
}
