using PyroCloud.Core.Application.Dtos.Identity.Company;
using PyroCloud.Core.Application.Dtos.Identity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyroCloud.Modules.Identity.Interfaces
{
    public interface ICompanyAppService
    {
        Task<IEnumerable<CompanyResponseDto>> GetCompanies();
        Task<CompanyResponseDto> GetCompanyByIdAsync(Guid id);
    }
}
