using Boilerplate.SSO.Host.Pages.Account;

namespace Boilerplate.SSO.Host.Extensions;

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
            ArgumentNullException.ThrowIfNullOrWhiteSpace(providerName, nameof(providerName));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(providerScheme, nameof(providerScheme));
            externalProviders.Add(new ExternalProvider
            {
                Name = provider["Name"],
                Scheme = provider["Scheme"]
            });
        }
        return externalProviders;

    }
}
