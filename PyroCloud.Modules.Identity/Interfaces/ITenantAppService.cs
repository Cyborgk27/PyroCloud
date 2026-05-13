using PyroCloud.Core.Application.Dtos.Identity.Roles;
using PyroCloud.Core.Application.Dtos.Identity.Tenant;

namespace PyroCloud.Modules.Identity.Interfaces
{
    public interface ITenantAppService
    {

        Task<IEnumerable<TenantDto>> GetAvaliableTenants();

        Task<TenantResponseDto> GetTenantByIdAsync(Guid id);

        Task<TenantDto> CreateTenant(TenantDto tenant);

        Task<TenantDto> UpdateTenant(TenantDto tenant);

        Task<UpdateTenantStatusDto> ToggleTenantActive(Guid id);
    }
}
