using Microsoft.AspNetCore.Authorization;
using PyroCloud.Core.Domain.Interfaces;

namespace PyroCloud.Shared.Infrastructure.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }
    public PermissionRequirement(string permission) => Permission = permission;
}

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly ICurrentUserProvider _currentUser;

    public PermissionAuthorizationHandler(ICurrentUserProvider currentUser)
    {
        _currentUser = currentUser;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!_currentUser.IsAuthenticated) return Task.CompletedTask;

        if (_currentUser.Roles.Contains("super_admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (_currentUser.Permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}