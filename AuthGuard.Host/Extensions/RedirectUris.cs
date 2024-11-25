using System;
using Microsoft.Extensions.Primitives;

namespace AuthGuard.Host.Extensions;

public static class RedirectUris
{
    public static string BuildForChallenge(HttpRequest request, List<string> excluding = null)
    {
        excluding ??= [];

        var parameters = request.HasFormContentType
            ? request.Form
                .Where(kv => !excluding.Contains(kv.Key))
                .ToDictionary(fk => fk.Key, fv => fv.Value)
            : request.Query
                .Where(v => !excluding.Contains(v.Key))
                .ToDictionary(v => v.Key, v => v.Value);
        var url = request.PathBase + request.Path + QueryString.Create(parameters);
        return url;
    }
}
