using Microsoft.AspNetCore.Authorization;

namespace GraphQLApi.Security;

public class AlphaTenantsRequirement : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var tenantIdClaim = context.User.FindFirst("TenantId")?.Value;
        if (tenantIdClaim is not ("123" or "789")) return Task.CompletedTask;
        foreach (var requirement in context.PendingRequirements.ToList())
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}