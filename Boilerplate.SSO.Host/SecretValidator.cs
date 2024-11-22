using System;
using System.Security.Cryptography;
using System.Text;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Boilerplate.SSO.Host;

public class SecretValidator : ISecretValidator
{
    public Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
    {
        if (parsedSecret.Type != IdentityServerConstants.ParsedSecretTypes.SharedSecret)
        {
            return Task.FromResult(new SecretValidationResult { Success = false });
        }

        var sharedSecret = parsedSecret.Credential as string;
        if (string.IsNullOrEmpty(sharedSecret))
        {
            return Task.FromResult(new SecretValidationResult { Success = false });
        }

        foreach (var secret in secrets)
        {
            if (secret.Type == IdentityServerConstants.SecretTypes.SharedSecret)
            {
                var hashedSecret = HashSecret(sharedSecret);
                if (TimeConstantComparer.IsEqual(hashedSecret, secret.Value))
                {
                    return Task.FromResult(new SecretValidationResult { Success = true });
                }
            }
        }

        return Task.FromResult(new SecretValidationResult { Success = false });
    }

    private string HashSecret(string secret)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(secret);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}

public static class TimeConstantComparer
{
    public static bool IsEqual(string a, string b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        var result = 0;
        for (int i = 0; i < a.Length; i++)
        {
            result |= a[i] ^ b[i];
        }

        return result == 0;
    }
}
