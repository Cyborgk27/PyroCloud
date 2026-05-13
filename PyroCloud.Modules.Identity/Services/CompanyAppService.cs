using Microsoft.EntityFrameworkCore;
using PyroCloud.Core.Application.Dtos.Identity.Company;
using PyroCloud.Core.Application.Dtos.Identity.Tenant;
using PyroCloud.Core.Application.Interfaces;
using PyroCloud.Core.Domain.Exceptions;
using PyroCloud.Modules.Identity.Interfaces;
using PyroCloud.Shared.Infrastructure.Presistence.Context;

namespace PyroCloud.Modules.Identity.Services
{
    public class CompanyAppService : ICompanyAppService
    {
        private readonly PyroDbContext _context;
        private readonly IFileAppService _file;


        public CompanyAppService(PyroDbContext context, IFileAppService file)
        {
            _context = context;
            _file = file;
        }

        public async Task<IEnumerable<CompanyResponseDto>>GetCompanies()
        {
            return await _context.Companies
                .AsNoTracking() // 1. Primero desactivas el tracking sobre la Entidad
                .Select(r => new CompanyResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                }) // 2. Luego proyectas al DTO
                .ToListAsync();
        }

        public async Task<CompanyResponseDto> GetCompanyByIdAsync(Guid companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);

            if (company == null)
                throw new UserFriendlyException("Company not found");

            return new CompanyResponseDto
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                Email = company.Email,
                PhoneNumber = company.PhoneNumber,
                //ImageUrl = tenant.ImageUrl
            };

        }

    }
}
