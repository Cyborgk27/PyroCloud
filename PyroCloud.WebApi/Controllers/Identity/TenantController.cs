using Microsoft.AspNetCore.Mvc;
using PyroCloud.Core.Application.Dtos.Identity.Roles;
using PyroCloud.Core.Application.Dtos.Identity.Tenant;
using PyroCloud.Modules.Identity.Interfaces;
using PyroCloud.Modules.Identity.Services;

namespace PyroCloud.WebApi.Controllers.Identity
{
    [ApiController]
    [Route("api/identity/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTenants()
        {
            var tenants = await _tenantAppService.GetAvaliableTenants();
            return Ok(tenants);
        }

        [HttpGet("/tenant/{id:guid}")]
        public async Task<IActionResult> GetTenantById(Guid id)
        {
            var tenant = await _tenantAppService.GetTenantByIdAsync(id);
            return Ok(tenant);
        }

        [HttpPost]
        public async Task<ActionResult<TenantDto>> CreateTenant([FromForm] TenantDto input)
        {
            // El servicio genera el Guid y guarda en la BD
            var result = await _tenantAppService.CreateTenant(input);
            return Ok(result);
        }
        
        [HttpPut]
        public async Task<IActionResult> EditTenant([FromForm] TenantDto dto)
        {
            var result = await _tenantAppService.UpdateTenant(dto);
            return Ok(result);
        }

        [HttpPatch("{id}/toggle-active-tenant")]
        public async Task<IActionResult> ToggleTenantActive(Guid id)
        {
            var result = await _tenantAppService.ToggleTenantActive(id);

            //// Retornamos un mensaje claro
            //var estado = result ? "activado" : "desactivado";
            return Ok(result );
        }
    }
}
