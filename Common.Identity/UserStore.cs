using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace Common.Identity;

public class UserStore :
IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>,
IUserEmailStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>, IUserTwoFactorStore<ApplicationUser>,
IUserLockoutStore<ApplicationUser>, IDisposable
{
    private bool isDisposed;
    private readonly SqlConnection connection = new();
    public UserStore(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }
        connection.ConnectionString = connectionString;
    }

    #region IUserStore Members
    public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {


        var sql = @"INSERT INTO Users (UserName, NormalizedUserName, Email, NormalizedEmail, PasswordHash,
         FirstName, MiddleName, LastName, DateOfBirth, Organization, PhoneNumber, ImageUrl) 
         VALUES (@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @PasswordHash, @FirstName,
          @MiddleName, @LastName, @DateOfBirth, @Organization, @PhoneNumber, @ImageUrl)";
        connection.Execute(sql, user);


        return Task.FromResult(IdentityResult.Success);
    }
    public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var sql = "DELETE FROM Users WHERE Id = @Id";
        connection.Execute(sql, new { user.Id });
        return Task.FromResult(IdentityResult.Success);
    }
    public Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM Users WHERE Id = @Id";
        var user = connection.QuerySingleOrDefault<ApplicationUser>(sql, new { Id = userId });
        return Task.FromResult(user);
    }
    public Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM Users WHERE NormalizedUserName = @NormalizedUserName";
        var user = connection.QuerySingleOrDefault<ApplicationUser>(sql, new { NormalizedUserName = normalizedUserName });
        return Task.FromResult(user);

    }
    
     Task<string> IUserStore<ApplicationUser>.GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }
    public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
       var sql = @"UPDATE Users SET UserName = @UserName, NormalizedUserName = @NormalizedUserName,
         Email = @Email, NormalizedEmail = @NormalizedEmail, PasswordHash = @PasswordHash, 
         FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, DateOfBirth = @DateOfBirth,
         Organization = @Organization, Mobile = @Mobile, ImageUrl = @ImageUrl WHERE Id = @Id";
       
       connection.ExecuteAsync(sql, user);
       return Task.FromResult(IdentityResult.Success);
    }


    #endregion

    #region IUserPasswordStore Members
    public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash != null);
    }
    #endregion

    #region IUserRoleStore Members
    public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {

        var sql = "INSERT INTO UserRoles (UserId, RoleName) VALUES (@UserId, @RoleName)";
        connection.Execute(sql, new { UserId = user.Id, RoleName = roleName });

        return Task.CompletedTask;
    }

    public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        var sql = "DELETE FROM UserRoles WHERE UserId = @UserId AND RoleName = @RoleName";
        connection.Execute(sql, new { UserId = user.Id, RoleName = roleName });
        return Task.CompletedTask;
    }

    public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var sql = "SELECT RoleName FROM UserRoles WHERE UserId = @UserId";
        var roles = connection.Query<string>(sql, new { UserId = user.Id }).AsList();
        return Task.FromResult<IList<string>>(roles);
    }

    public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        var sql = "SELECT COUNT(*) FROM UserRoles WHERE UserId = @UserId AND RoleName = @RoleName";
        var count = connection.ExecuteScalar<int>(sql, new { UserId = user.Id, RoleName = roleName });
        return Task.FromResult(count > 0);
    }

    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        var sql = "SELECT u.* FROM Users u INNER JOIN UserRoles ur ON u.Id = ur.UserId WHERE ur.RoleName = @RoleName";
        var users = connection.Query<ApplicationUser>(sql, new { RoleName = roleName }).AsList();
        return Task.FromResult<IList<ApplicationUser>>(users);
    }

    public Task<string> GetUserIdAsync(ApplicationRole user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetUserNameAsync(ApplicationRole user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetUserNameAsync(ApplicationRole user, string? userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationRole user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(ApplicationRole user, string? normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> CreateAsync(ApplicationRole user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(ApplicationRole user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(ApplicationRole user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region IUserEmailStore Members
    public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
    {
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM Users WHERE NormalizedEmail = @NormalizedEmail";
        var user = connection.QuerySingleOrDefault<ApplicationUser>(sql, new { NormalizedEmail = normalizedEmail });
        return Task.FromResult(user);
    }

    public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }
    #endregion
    #region IUserPhoneNumberStore Members
    public Task SetPhoneNumberAsync(ApplicationUser user, string? phoneNumber, CancellationToken cancellationToken)
    {
        user.PhoneNumber = phoneNumber;
        return Task.CompletedTask;
    }

    public Task<string?> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PhoneNumber);
    }

    public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        user.PhoneNumberConfirmed = confirmed;
        return Task.CompletedTask;
    }

    #endregion
    #region IUserTwoFactorStore Members
    public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
    {
        user.TwoFactorEnabled = enabled;
        return Task.CompletedTask;
    }

    public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.TwoFactorEnabled);
    }
    #endregion
    #region IuserLockoutStore Members
    public Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.LockoutEnd);
    }

    public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
    {
        user.LockoutEnd = lockoutEnd;
        return Task.CompletedTask;
    }

    public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        user.AccessFailedCount++;
        return Task.FromResult(user.AccessFailedCount);
    }

    public Task ResetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        user.AccessFailedCount = 0;
        return Task.CompletedTask;
    }

    public Task<int> GetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.AccessFailedCount);
    }

    public Task<bool> GetLockoutEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.LockoutEnabled);
    }

    public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
    {
        user.LockoutEnabled = enabled;
        return Task.CompletedTask;
    }

    #endregion
    #region IDisposable Support
    ~UserStore()
    {
        Dispose(false);
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

   

    #endregion

}
