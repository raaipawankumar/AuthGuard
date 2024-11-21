using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace Common.Identity;

public class RoleStore : IRoleStore<ApplicationRole>
{
    private bool isDisposed = false;
    private readonly SqlConnection connection = new();

    public RoleStore(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }
        connection.ConnectionString = connectionString;
    }

    public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        var sql = @"INSERT INTO Roles (Name, NormalizedName) 
        VALUES ( @Name, @NormalizedName)";
        connection.Execute(sql, role);
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        var sql = "DELETE FROM Roles WHERE Id = @Id";
        connection.Execute(sql, new { role.Id });
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<ApplicationRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM Roles WHERE Id = @Id";
        var role = connection.QuerySingleOrDefault<ApplicationRole>(sql, new { Id = roleId });
        return Task.FromResult(role);
    }

    public Task<ApplicationRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM Roles WHERE NormalizedName = @NormalizedName";
        var role = connection.QuerySingleOrDefault<ApplicationRole>(sql, new { NormalizedName = normalizedRoleName });
        return Task.FromResult(role);
    }

    public Task<string?> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.NormalizedName);
    }

    public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string?> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }

    public Task SetNormalizedRoleNameAsync(ApplicationRole role, string? normalizedName, CancellationToken cancellationToken)
    {
        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetRoleNameAsync(ApplicationRole role, string? roleName, CancellationToken cancellationToken)
    {
        role.Name = roleName;
        return Task.CompletedTask;
    }

    public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        var sql = "UPDATE Roles SET Name = @Name, NormalizedName = @NormalizedName WHERE Id = @Id";
        connection.Execute(sql, role);
        return Task.FromResult(IdentityResult.Success);
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed) return;
        if (disposing)
        {
            connection?.Dispose();
        }
        isDisposed = true;
    }
}