using PyroCloud.Core.Application.Authorization;
using PyroCloud.Core.Application.Common;
using PyroCloud.Modules.Identity.Interfaces;

namespace PyroCloud.Modules.Identity.Services
{
    public class RoleAppService : IRoleAppService
    {
        
        public IEnumerable<PermissionDto> GetAvaliablePermissions()
        {
            return PermissionRegistry.GetAll();
        }
    }
}
