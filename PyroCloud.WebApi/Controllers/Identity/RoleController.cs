using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
