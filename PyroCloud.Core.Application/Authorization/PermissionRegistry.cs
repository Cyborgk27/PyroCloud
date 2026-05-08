using PyroCloud.Core.Application.Common;

namespace PyroCloud.Core.Application.Authorization;

public static class PermissionRegistry
{
    private static readonly List<PermissionDto> _modules = new();

    public static void AddModulePermissions(PermissionDto modulePermission)
    {
        if (!_modules.Any(x => x.Value == modulePermission.Value))
        {
            _modules.Add(modulePermission);
        }
    }

    public static List<PermissionDto> GetAll() => _modules;
}