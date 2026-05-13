using Microsoft.EntityFrameworkCore;
using PyroCloud.Core.Application.Dtos.Identity.Tenant;
using PyroCloud.Core.Application.Interfaces;
using PyroCloud.Core.Domain.Entities.Identity;
using PyroCloud.Core.Domain.Exceptions;
using PyroCloud.Modules.Identity.Interfaces;
using PyroCloud.Shared.Infrastructure.Presistence.Context;

namespace PyroCloud.Modules.Identity.Services
{
    public class TenantAppService : ITenantAppService
    {

        private readonly PyroDbContext _context;
        private readonly IFileAppService _file;


        public TenantAppService(PyroDbContext context, IFileAppService file)
        {
            _context = context;
            _file = file;
        }

        public async Task<IEnumerable<TenantDto>> GetAvaliableTenants()
        {
            return await _context.Tenants
                .AsNoTracking() // 1. Primero desactivas el tracking sobre la Entidad
                .Select(r => new TenantDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                }) // 2. Luego proyectas al DTO
                .ToListAsync();
        }

        public async Task<TenantResponseDto> GetTenantByIdAsync(Guid tenantId)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            
            if (tenant == null)
                throw new UserFriendlyException("Tenant not found");

            return new TenantResponseDto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Description = tenant.Description,
                Code = tenant.Code,
                Email = tenant.Email,
                ImageUrl = tenant.ImageUrl,
                PhoneNumber = tenant.PhoneNumber,
            };

        }

        public async Task<TenantDto> CreateTenant(TenantDto tenant)
        {
            var imageUrl = await _file.SaveFileAsync(tenant.ImageUrl, "tenants");

            var newTenant = new Tenant
            {
                Name = tenant.Name,
                Email = tenant.Email,
                PhoneNumber = tenant.PhoneNumber,
                Description = tenant.Description,
                Code = tenant.Code,
                ImageUrl = imageUrl,
            };
            _context.Tenants.Add(newTenant);
            await _context.SaveChangesAsync();

            return new TenantDto
            {
                Id = newTenant.Id,
                Name = newTenant.Name,
                Description = newTenant.Description,
                Email = newTenant.Email,
                PhoneNumber = newTenant.PhoneNumber,
                ImageUrl = tenant.ImageUrl,
                Code = newTenant.Code
            };
        }

        public async Task<TenantDto> UpdateTenant(TenantDto tenant)
        {
            var existingTenant = await _context.Tenants.FirstOrDefaultAsync(r => r.Id == tenant.Id);

            if (existingTenant == null)
                throw new UserFriendlyException("Tenant not found");

            if (tenant.ImageUrl is not null)
            {
                var imageUrl = await _file.SaveFileAsync(tenant.ImageUrl, "tenants");
                existingTenant.ImageUrl = imageUrl;
            }

            
            existingTenant.Name = tenant.Name;
            existingTenant.Email = tenant.Email;
            existingTenant.PhoneNumber = tenant.PhoneNumber;
            existingTenant.Description = tenant.Description;

            await _context.SaveChangesAsync();
            return new TenantDto
            {
                Id = existingTenant.Id,
                Name = existingTenant.Name,
                Email = existingTenant.Email,
                PhoneNumber = existingTenant.PhoneNumber,
                Description = existingTenant.Description,
                ImageUrl = tenant.ImageUrl,
            };
        }

        public async Task<UpdateTenantStatusDto> ToggleTenantActive(Guid id)
        {
            // 1. Buscamos la entidad real
            var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == id);

            if (tenant == null) throw new UserFriendlyException("Tenant no encontrado");

            // 2. Modificamos la entidad
            tenant.IsActive = !tenant.IsActive;
            await _context.SaveChangesAsync();

            // 3. MOMENTO DEL DTO: Mapeamos la entidad al DTO de salida
            return new UpdateTenantStatusDto
            {
                Id = tenant.Id,
                IsActive = tenant.IsActive
            };
        }

    }
}
