using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PyroCloud.Core.Application.Dtos.Identity.Roles;
using PyroCloud.Modules.Identity.Authorization;
using PyroCloud.Modules.Identity.Interfaces;
using PyroCloud.Shared.Infrastructure.Authorization;

namespace PyroCloud.WebApi.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleAppService _roleAppService;

        public RoleController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        [HttpGet("permissions")]
        [HasPermission(IdentityPermissions.Roles.Manage)]
        public IActionResult GetAvaliablePermissions()
        {
            var permissions = _roleAppService.GetAvaliablePermissions();
            return Ok(permissions);
        }

        [HttpGet]
        [HasPermission(IdentityPermissions.Roles.Default)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleAppService.GetAvaliableRoles();
            return Ok(roles);
        }

        [HttpGet("{id:int}")]
        [HasPermission(IdentityPermissions.Roles.Default)]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleAppService.GetRoleByIdAsync(id);
            return Ok(role);
        }

        [HttpPost]
        [HasPermission(IdentityPermissions.Roles.Manage)]
        public async Task<IActionResult> RegisterRole(RoleDto dto)
        {
            var role = await _roleAppService.CreateRole(dto);
            return Ok(role);
        }

        [HttpPut]
        [HasPermission(IdentityPermissions.Roles.Manage)]
        public async Task<IActionResult> EditRole(RoleDto dto)
        {
            var role = await _roleAppService.UpdateRole(dto);
            return Ok(role);
        }
    }
}