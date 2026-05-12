using Microsoft.EntityFrameworkCore;
using PyroCloud.Core.Application.Authorization;
using PyroCloud.Core.Application.Common;
using PyroCloud.Core.Application.Dtos.Identity.Roles;
using PyroCloud.Core.Domain.Entities.Identity;
using PyroCloud.Core.Domain.Exceptions;
using PyroCloud.Modules.Identity.Interfaces;
using PyroCloud.Shared.Infrastructure.Presistence.Context;

namespace PyroCloud.Modules.Identity.Services
{
    public class RoleAppService : IRoleAppService
    {

        private readonly PyroDbContext _context;

        public RoleAppService(PyroDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PermissionDto> GetAvaliablePermissions()
        {
            return PermissionRegistry.GetAll();
        }

        public async Task<IEnumerable<RoleDto>> GetAvaliableRoles()
        {
            return await _context.Roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                ShowName = r.ShowName,
                Description = r.Description,
            })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RoleDto> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles
                .Include(x => x.RolePermissions.Where(x => x.IsDeleted == false))
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
                throw new UserFriendlyException("Role not found");
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                ShowName = role.ShowName,
                Description = role.Description,
                Permissions = role.RolePermissions.Select(rp => rp.Permission)
            };
        }

        public async Task<RoleDto> CreateRole(RoleDto role)
        {
            var newRole = new Role
            {
                Name = role.Name,
                ShowName = role.ShowName,
                Description = role.Description,
                RolePermissions = role.Permissions.Select(p => new RolePermission
                {
                    Permission = p
                }).ToList()
            };
            _context.Roles.Add(newRole);
            await _context.SaveChangesAsync();

            return new RoleDto
            {
                Id = newRole.Id,
                Name = newRole.Name,
                ShowName = newRole.ShowName,
                Description = newRole.Description,
                Permissions = newRole.RolePermissions.Select(rp => rp.Permission)
            };
        }

        public async Task<RoleDto> UpdateRole(RoleDto role)
        {
            var existingRole = await _context.Roles
                .Include(r => r.RolePermissions)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == role.Id);

            if (existingRole == null)
                throw new UserFriendlyException("Role not found");

            existingRole.ShowName = role.ShowName;
            existingRole.Description = role.Description;

            var permissionsToSoftDelete = existingRole.RolePermissions
                .Where(rp => !rp.IsDeleted && !role.Permissions.Contains(rp.Permission))
                .ToList();

            foreach (var rp in permissionsToSoftDelete)
            {
                _context.RolePermissions.Remove(rp);
            }

            foreach (var permissionName in role.Permissions)
            {
                var existingRp = existingRole.RolePermissions
                    .FirstOrDefault(rp => rp.Permission == permissionName);

                if (existingRp == null)
                {
                    existingRole.RolePermissions.Add(new RolePermission
                    {
                        Permission = permissionName,
                        RoleId = existingRole.Id
                    });
                }
                else if (existingRp.IsDeleted)
                {
                    existingRp.IsDeleted = false;
                    existingRp.DeletedBy = null;
                    existingRp.DeletedDate = null;
                    _context.Entry(existingRp).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();

            return new RoleDto
            {
                Id = existingRole.Id,
                Name = existingRole.Name,
                ShowName = existingRole.ShowName,
                Description = existingRole.Description,
                Permissions = role.Permissions
            };
        }
        public async Task<RoleDto> DeleteRole(int id)
        {
            var existingRole = await _context.Roles.FindAsync(id);
            if (existingRole == null)
                throw new UserFriendlyException("Role not found");
            _context.Roles.Remove(existingRole);
            await _context.SaveChangesAsync();
            return new RoleDto
            {
                Id = existingRole.Id,
                Name = existingRole.Name,
                ShowName = existingRole.ShowName,
                Description = existingRole.Description,
                Permissions = existingRole.RolePermissions.Select(rp => rp.Permission)
            };

        }
    }
}
