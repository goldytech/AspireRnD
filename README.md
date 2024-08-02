# .NET Aspire Solution

This repository contains a .NET Aspire solution with multiple projects. Below is a detailed description of each project, including their purpose, referenced NuGet packages, and project references.

## Projects

### 1. GraphQLApi

**Description:**
The `GraphQLApi` project is responsible for providing a GraphQL API. It includes endpoints for authentication and querying data from a MongoDB database.

**NuGet Packages:**
- `HotChocolate.AspNetCore`
- `HotChocolate.Authorization`
- `MongoDB.Driver`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.IdentityModel.Tokens`
- `Asp.Versioning`
- `Swashbuckle.AspNetCore`

**Project References:**
- `ServiceDefaults`

**Key Files:**
- `Program.cs`: Configures services and middleware for the application.
- `AuthEndpoints.cs`: Contains the authentication endpoints.
- `Extensions.cs`: Contains extension methods for configuring services.
- `AlphaBroderProduct.cs`: Defines the `AlphaBroderProduct` entity.

**Authorization Policy:**
The `GraphQLApi` project uses an authorization policy named `AlphaTenantsPolicy`. This policy ensures that only authenticated users with specific tenant IDs can access certain resources.

**Policy Logic:**
- The policy requires the user to be authenticated.
- It checks for a claim named `TenantId`.
- It validates that the `TenantId` claim is one of the allowed tenant IDs (`123` or `789`).

The policy is defined in the `Extensions.cs` file:

```csharp
public static void AddAuthorizationServices(this WebApplicationBuilder webApplicationBuilder)
{
    var allowedAlphaTenantIds = new List<string> { "123", "789" };
    webApplicationBuilder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AlphaTenantsPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireAssertion(context =>
            {
                var tenantIdClaim = context.User.FindFirst("TenantId")?.Value;
                return tenantIdClaim is not null && allowedAlphaTenantIds.Contains(tenantIdClaim);
            });
        });
    });
}