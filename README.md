[![](https://img.shields.io/nuget/v/soenneker.extensions.webapplicationfactories.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.webapplicationfactories/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.webapplicationfactories/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.webapplicationfactories/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.webapplicationfactories.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.webapplicationfactories/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.webapplicationfactories/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.webapplicationfactories/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.WebApplicationFactories
A collection of helpful WebApplicationFactory extension methods.

## Installation

```bash
dotnet add package Soenneker.Extensions.WebApplicationFactories
```

## Quick start

```csharp
using Soenneker.Extensions.WebApplicationFactories;

// Given an existing WebApplicationFactory<T> named factory:
var result = factory.CreateTestHttpClient();
```

## Common operations

- `CreateTestHttpClient()` - Creates a new HttpClient instance configured for integration testing with optional test authentication headers. Returns a new HttpClient instance configured with the specified authentication headers for use in integration tests.
