using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Framework;

namespace PetFamily.Accounts.Infrastructure;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionRequirementHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionAttribute requirement)
    {
        var claims = context.User.Claims;
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.Id);
        
        if (userId is null)
            return;

        context.Succeed(requirement);
    }
}