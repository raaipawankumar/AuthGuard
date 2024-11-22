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

    public Task<Client?> FindClientByIdAsync(string clientId)
    {

  
            connection.Open();
            Client? client = null;
            using (var command = new SqlCommand("SELECT * FROM Clients WHERE ClientId = @ClientId", connection))
            {
                command.Parameters.AddWithValue("@ClientId", clientId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                client = new Client
                {
                    ClientId = reader["ClientId"] as string ?? string.Empty,
                            ClientName = reader["ClientName"] as string ?? string.Empty,
                            // LogoUri = reader["LogoUri"] as string ?? string.Empty,
                            // RequireConsent = reader["RequireConsent"] != DBNull.Value && Convert.ToBoolean(reader["RequireConsent"]),
                            // AllowRememberConsent = reader["AllowRememberConsent"] != DBNull.Value && Convert.ToBoolean(reader["AllowRememberConsent"]),
                            AllowedGrantTypes = reader["AllowedGrantTypes"] != DBNull.Value
                                ? (reader["AllowedGrantTypes"] as string ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                : new List<string>()
                  
                };
            }
        }


        if (client == null) return Task.FromResult(client);

        client.ClientSecrets = (connection.Query<Secret>(
            "SELECT * FROM ClientSecrets WHERE ClientId = @ClientId",
             new { ClientId = clientId }))
             .ToList();

        client.AllowedScopes = connection.Query<string>(
            "SELECT Scope FROM ClientScopes WHERE ClientId = @ClientId", new { ClientId = clientId }).ToList();
          

        return Task.FromResult<Client?>(client);

    }
}
