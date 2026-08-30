[![](https://img.shields.io/nuget/v/soenneker.extensions.webapplicationfactories.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.webapplicationfactories/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.webapplicationfactories/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.webapplicationfactories/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.webapplicationfactories.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.webapplicationfactories/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.webapplicationfactories/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.webapplicationfactories/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.WebApplicationFactories
Create an integration-test `HttpClient` configured for either a real bearer token or Soenneker's test-authentication headers.

## Installation

```bash
dotnet add package Soenneker.Extensions.WebApplicationFactories
```

## Use test authentication

```csharp
using Soenneker.Extensions.WebApplicationFactories;

using HttpClient client = factory.CreateTestHttpClient(
    userId: "user-42",
    email: "ada@example.com",
    userRoles: ["Admin", "Support"]);

HttpResponseMessage response = await client.GetAsync("/api/account");
```

Without a JWT, the client sends:

```text
Authorization: Test
AuthorizationUserId: user-42
AuthorizationEmail: ada@example.com
AuthorizationRoles: Admin,Support
```

Null and empty identity values are omitted. Roles are joined with commas in the supplied order. Header values use the runtime's validated header API, so values containing invalid newline characters are rejected rather than sent.

The application under test must explicitly register a test authentication handler that understands this scheme and these headers. These headers do not authenticate a client by themselves. Never enable a header-trusting test handler in a production environment.

## Use a bearer token

```csharp
using HttpClient client = factory.CreateTestHttpClient(jwt: accessToken);
```

When `jwt` has content, it takes precedence: the client sends `Authorization: Bearer <token>` and none of the custom identity headers. The helper does not create or validate the token.

Each call uses `WebApplicationFactory<T>.CreateClient()` and returns a new client owned by the caller. Dispose the client after the test. The factory still controls the in-memory test server and its configured client options.
