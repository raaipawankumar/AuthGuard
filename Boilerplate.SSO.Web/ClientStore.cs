using Microsoft.Data.SqlClient;
using IdentityServer4.Stores;
using IdentityServer4.Models;
using Dapper;

namespace Boilerplate.SSO.Web;

public class ClientStore : IClientStore
{
    private readonly SqlConnection connection;

    public ClientStore(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }
        connection = new SqlConnection(connectionString);
    }

    public async Task<Client?> FindClientByIdAsync(string clientId)
    {

        var client = await connection.QuerySingleOrDefaultAsync<Client>(
            "SELECT * FROM Clients WHERE ClientId = @ClientId",
             new { ClientId = clientId });

        if (client == null) return client;

        client.ClientSecrets = (await connection.QueryAsync<Secret>(
            "SELECT * FROM ClientSecrets WHERE ClientId = @ClientId",
             new { ClientId = clientId }))
             .ToList();

        client.AllowedScopes = (await connection.QueryAsync<string>(
            "SELECT Scope FROM ClientScopes WHERE ClientId = @ClientId", new { ClientId = clientId }))
            .ToList();

        return client;

    }
}
