using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PyroCloud.Core.Application.Dtos.Identity.Roles;
using PyroCloud.Modules.Identity.Interfaces;

namespace PyroCloud.WebApi.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleAppService _roleAppService;

        public RoleController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        [HttpGet("permissions")]
        public IActionResult GetAvaliablePermissions()
        {
            var permissions = _roleAppService.GetAvaliablePermissions();
            return Ok(permissions);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleAppService.GetAvaliableRoles();
            return Ok(roles);
        }

        [HttpGet("/{id:int}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleAppService.GetRoleByIdAsync(id);
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterRole(RoleDto dto)
        {
            var role = await _roleAppService.CreateRole(dto);
            return Ok(role);
        }

        [HttpPut]
        public async Task<IActionResult> EditRole(RoleDto dto)
        {
            var role = await _roleAppService.UpdateRole(dto);
            return Ok(role);
        }
    }
}
