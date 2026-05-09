using PyroCloud.Core.Application.Common;
using PyroCloud.Core.Application.Dtos.Identity.Roles;

namespace PyroCloud.Modules.Identity.Interfaces
{
    public interface IRoleAppService
    {
        IEnumerable<PermissionDto> GetAvaliablePermissions();

        Task<IEnumerable<RoleDto>> GetAvaliableRoles();

        Task<RoleDto> GetRoleByIdAsync(int id);

        Task<RoleDto> CreateRole(RoleDto role);

        Task<RoleDto> UpdateRole(RoleDto role);

        Task<RoleDto> DeleteRole(int id);
    }
}
