using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Soenneker.Enums.DeployEnvironment;
using Soenneker.Extensions.Enumerable.String;
using Soenneker.Extensions.String;

namespace Soenneker.Extensions.WebApplicationFactories;

/// <summary>
/// A collection of helpful WebApplicationFactory extension methods
/// </summary>
public static class WebApplicationFactoriesExtension
{
    /// <summary>
    /// Creates a configured <see cref="HttpClient"/> instance for integration testing,
    /// with built-in authentication headers.
    /// If <paramref name="jwt"/> is provided, it overrides all other authentication parameters.
    /// </summary>
    /// <typeparam name="T">The entry point class for the web application.</typeparam>
    /// <param name="factory">The test web application factory.</param>
    /// <param name="userId">Optional user ID for authentication headers (used if JWT is not provided).</param>
    /// <param name="email">Optional email for authentication headers (used if JWT is not provided).</param>
    /// <param name="jwt">Optional JWT token to be used directly for authentication.</param>
    /// <param name="userRoles">Optional list of roles for the user.</param>
    /// <returns>A configured <see cref="HttpClient"/> instance.</returns>
    public static HttpClient CreateTestHttpClient<T>(this WebApplicationFactory<T> factory, string? userId = null, string? email = null, string? jwt = null,
        List<string>? userRoles = null) where T : class
    {
        HttpClient client = factory.CreateClient();

        if (jwt.HasContent())
        {
            // Use the provided JWT for bearer token auth
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            return client;
        }

        // Use custom header-based test authentication
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(DeployEnvironment.Test.Name);

        if (userId.HasContent())
            client.DefaultRequestHeaders.Add("AuthorizationUserId", userId);

        if (email.HasContent())
            client.DefaultRequestHeaders.Add("AuthorizationEmail", email);

        if (userRoles is {Count: > 0})
        {
            // Avoid unnecessary allocation when userRoles is null or empty
            string roles = userRoles.ToCommaSeparatedString();
            client.DefaultRequestHeaders.Add("AuthorizationRoles", roles);
        }

        return client;
    }
}