using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PyroCloud.Core.Application.Authorization;
using PyroCloud.Core.Application.Common;
using PyroCloud.Core.Application.Dtos.Setup;
using PyroCloud.Core.Domain.Entities.Identity;
using PyroCloud.Core.Domain.Interfaces;
using PyroCloud.Shared.Infrastructure.Presistence.Context;

namespace PyroCloud.Shared.Infrastructure.Setup;

public class SeedDataService
{
    private readonly PyroDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;

    public SeedDataService(PyroDbContext context, IConfiguration configuration, IPasswordHasher passwordHasher)
    {
        _context = context;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        var options = _configuration.GetSection("SeedData").Get<SeedDataOptions>();
        if (options == null) return;

        var tenant = await SeedTenantAsync(options.DefaultTenant);

        await SeedRolesAsync(options.Roles);

        await SeedUsersAsync(options.Users);

        await SyncSuperAdminPermissionsAsync();
    }

    private async Task<Tenant> SeedTenantAsync(TenantSeedDto dto)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Code == dto.Code);
        if (tenant == null)
        {
            tenant = new Tenant { Name = dto.Name, Code = dto.Code, Description = dto.Description };
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();
        }
        return tenant;
    }

    private async Task SeedRolesAsync(List<RoleSeedDto> roles)
    {
        foreach (var dto in roles)
        {
            if (!await _context.Roles.AnyAsync(r => r.Name == dto.Name))
            {
                _context.Roles.Add(new Role
                {
                    Name = dto.Name,
                    ShowName = dto.ShowName,
                    Description = dto.Description
                });
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedUsersAsync(List<UserSeedDto> users)
    {
        foreach (var dto in users)
        {
            if (!await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                var user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    PasswordHash = _passwordHasher.Hash(dto.Password),
                };

                await _context.Users.AddAsync(user);

                var roles = await _context.Roles
                    .Where(r => dto.Roles.Contains(r.Name))
                    .ToListAsync();

                var userRoles = roles.Select(r => new UserRole { User = user, Role = r }).ToList();
                await _context.UserRoles.AddRangeAsync(userRoles);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SyncSuperAdminPermissionsAsync()
    {
        var allPermissions = PermissionRegistry.GetAll()
            .SelectMany(GetFlatPermissions)
            .ToList();

        var superAdminRole = await _context.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Name == "super_admin");

        if (superAdminRole == null) return;

        foreach (var permissionValue in allPermissions)
        {
            if (!superAdminRole.RolePermissions.Any(rp => rp.Permission == permissionValue))
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = superAdminRole.Id,
                    Permission = permissionValue
                });
            }
        }

        await _context.SaveChangesAsync();
    }

    private IEnumerable<string> GetFlatPermissions(PermissionDto permission)
    {
        yield return permission.Value;
        foreach (var child in permission.Children)
        {
            foreach (var subChild in GetFlatPermissions(child))
            {
                yield return subChild;
            }
        }
    }
}