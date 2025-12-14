using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/department")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class DepartmentController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public DepartmentController(IServiceManager serviceManager) => _serviceManager = serviceManager;
        [HttpGet]
        public async Task<IActionResult> GetDepartmentsForCompany(Guid companyId)
        {
            var departments = await _serviceManager.departmentService.GetDepartmentsAsync(companyId, false);
            return Ok(departments);
        }
        [HttpGet]
        [Route("{departmentId}")]
        public async Task<IActionResult> GetDepartmentForCompany(Guid companyId, Guid departmentId)
        {
            var department = await _serviceManager.departmentService.GetDepartmentAsync(companyId, departmentId, false);
            return Ok(department);
        }
        [HttpDelete]
        [Route("{departmentId}")]
        public async Task<IActionResult> DeleteDepartmentforCompany(Guid companyId, Guid departmentId)
        {
            await _serviceManager.departmentService.DeleteDepartmentForCompanyAsync(companyId, departmentId, false);
            return NoContent();
        }
        [HttpPut]
        [Route("{departmentId}")]
        public async Task<IActionResult> UpdateDepartmentForCompany(Guid companyId, Guid departmentId, [FromBody] DepartmentUpdateDto departmentForUpdate)
        {
            await _serviceManager.departmentService.UpdateDepartmentForCompanyAsync(companyId, departmentId, departmentForUpdate, true);
            return NoContent();
        }
    }
}
