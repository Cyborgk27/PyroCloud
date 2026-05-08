using PyroCloud.Core.Application.Common;

namespace PyroCloud.Modules.Identity.Interfaces
{
    public interface IRoleAppService
    {
        IEnumerable<PermissionDto> GetAvaliablePermissions();
    }
}
