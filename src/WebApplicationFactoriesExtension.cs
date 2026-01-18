using Microsoft.AspNetCore.Mvc.Testing;
using Soenneker.Enums.DeployEnvironment;
using Soenneker.Extensions.String;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

/// <summary>
/// Provides extension methods for configuring HttpClient instances with test authentication headers for integration
/// testing scenarios using WebApplicationFactory.
/// </summary>
/// <remarks>These extensions are intended to simplify the setup of authenticated HTTP clients in integration
/// tests. They support both JWT-based authentication and custom test authentication headers to simulate various user
/// identities and roles.</remarks>
public static class WebApplicationFactoriestExtensions
{
    private const string AuthorizationUserIdHeader = "AuthorizationUserId";
    private const string AuthorizationEmailHeader = "AuthorizationEmail";
    private const string AuthorizationRolesHeader = "AuthorizationRoles";

    // Cached because scheme is constant for all calls in test-auth mode.
    private static readonly AuthenticationHeaderValue TestAuthHeader =
        new AuthenticationHeaderValue(DeployEnvironment.Test.Name);

    /// <summary>
    /// Creates a new HttpClient instance configured for integration testing with optional test authentication headers.
    /// </summary>
    /// <remarks>If a JWT is provided, the client is configured to use Bearer authentication with the
    /// specified token. Otherwise, test authentication headers are added to simulate an authenticated user with the
    /// specified user ID, email, and roles. This method is intended for use in integration tests that require
    /// authenticated requests.</remarks>
    /// <typeparam name="T">The entry point of the ASP.NET Core application under test.</typeparam>
    /// <param name="factory">The WebApplicationFactory used to create the HttpClient instance.</param>
    /// <param name="userId">An optional user identifier to include in the test authentication headers. If null, the header is omitted.</param>
    /// <param name="email">An optional email address to include in the test authentication headers. If null, the header is omitted.</param>
    /// <param name="jwt">An optional JSON Web Token (JWT) to use for Bearer authentication. If specified, the client will use this token
    /// and will not include test authentication headers.</param>
    /// <param name="userRoles">An optional list of user roles to include in the test authentication headers. If null or empty, the roles header
    /// is omitted.</param>
    /// <returns>A new HttpClient instance configured with the specified authentication headers for use in integration tests.</returns>
    public static HttpClient CreateTestHttpClient<T>(
        this WebApplicationFactory<T> factory,
        string? userId = null,
        string? email = null,
        string? jwt = null,
        IReadOnlyList<string>? userRoles = null)
        where T : class
    {
        HttpClient client = factory.CreateClient();

        // JWT path: allocation here is fine; jwt varies so can't cache the full header.
        if (jwt.HasContent())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            return client;
        }

        // Test-auth path: reuse cached header value.
        client.DefaultRequestHeaders.Authorization = TestAuthHeader;

        var headers = client.DefaultRequestHeaders;

        if (userId.HasContent())
            headers.TryAddWithoutValidation(AuthorizationUserIdHeader, userId);

        if (email.HasContent())
            headers.TryAddWithoutValidation(AuthorizationEmailHeader, email);

        if (userRoles is { Count: > 0 })
        {
            string roles = userRoles.Count == 1
                ? userRoles[0]
                : string.Join(',', userRoles);

            if (roles.HasContent())
                headers.TryAddWithoutValidation(AuthorizationRolesHeader, roles);
        }

        return client;
    }
}
