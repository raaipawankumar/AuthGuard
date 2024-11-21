using Dapper;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Boilerplate.SSO.Web
{
    public class ResourceStore : IResourceStore
    {
        private readonly SqlConnection connection;

        public ResourceStore(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            connection = new SqlConnection(connectionString);
        }

        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {

            var scopes = connection.Query<ApiScope>(
                "SELECT * FROM ApiScopes WHERE Name IN @Names",
                new { Names = scopeNames });

            return Task.FromResult(scopes);

        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var resources = connection.Query<ApiResource>(
                   "SELECT * FROM ApiResources WHERE Name IN @Names",
                   new { Names = apiResourceNames });

            return Task.FromResult(resources);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = connection.Query<ApiResource>(
                     "SELECT ar.* FROM ApiResources ar JOIN ApiResourceScopes ars ON ar.Name = ars.ApiResourceName WHERE ars.ScopeName IN @Names",
                     new { Names = scopeNames });

            return Task.FromResult(resources);
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByNameAsync(IEnumerable<string> identityResourceNames)
        {
            var resources = connection.Query<IdentityResource>(
                    "SELECT * FROM IdentityResources WHERE Name IN @Names",
                    new { Names = identityResourceNames });

            return Task.FromResult(resources);
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var identityResources = connection.Query<IdentityResource>("SELECT * FROM IdentityResources");
            var apiResources = connection.Query<ApiResource>("SELECT * FROM ApiResources");
            var apiScopes = connection.Query<ApiScope>("SELECT * FROM ApiScopes");

            var resources = new Resources(identityResources, apiResources, apiScopes);
            return Task.FromResult(resources);
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = connection.Query<IdentityResource>(
                    "SELECT ir.* FROM IdentityResources ir JOIN IdentityResourceScopes irs ON ir.Name = irs.IdentityResourceName WHERE irs.ScopeName IN @Names",
                    new { Names = scopeNames });

            return Task.FromResult(resources);
        }
    }
}