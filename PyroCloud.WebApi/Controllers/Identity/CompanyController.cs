using Microsoft.AspNetCore.Mvc;
using PyroCloud.Modules.Identity.Interfaces;
using PyroCloud.Modules.Identity.Services;

namespace PyroCloud.WebApi.Controllers.Identity
{
    [ApiController]
    [Route("api/identity/[controller]")]
    public class CompanyController: ControllerBase
    {
        private readonly ICompanyAppService _companyAppService;

        public CompanyController(ICompanyAppService companyAppService)
        {
            _companyAppService = companyAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyAppService.GetCompanies();
            return Ok(companies);
        }

        [HttpGet("/company/{id:guid}")]
        public async Task<IActionResult> GetTenantById(Guid id)
        {
            var company = await _companyAppService.GetCompanyByIdAsync(id);
            return Ok(company);
        }
    }
}
