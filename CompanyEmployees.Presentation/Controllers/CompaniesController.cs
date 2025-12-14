using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/Companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController :ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public CompaniesController(IServiceManager serviceManager) => _serviceManager = serviceManager;

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _serviceManager.companyService.GetAllCompanies(trackChanges: false);
            return Ok(companies);
        }
        [HttpGet("{id:guid}",Name ="ComapnyById")]
        public async Task<IActionResult> GetCompany(Guid id )
        {
            var company = await _serviceManager.companyService.GetCompany(id, false);
            return Ok(company);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var companyResponse =await _serviceManager.companyService.CreateCompany(company);
            return CreatedAtRoute("CompanyById", new {id = companyResponse.Id},companyResponse);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
           await _serviceManager.companyService.DeleteCompany(id, false);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyDto company)
        {
            if (company is null)
                return BadRequest("CompanyDto object is null");
            await _serviceManager.companyService.UpdateCompany(company);
            return NoContent();
        }

    }
}
