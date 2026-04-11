using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class EmployeesController:ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public EmployeesController(IServiceManager serviceManager)=> _serviceManager = serviceManager;

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(int companyId)
        {
            var employees = await _serviceManager.employeeService.GetEmployees(companyId, false);
            return Ok(employees);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeForCompany(int companyId , int id)
        {
            var employee =await _serviceManager.employeeService.GetEmployee(companyId,id,false);
            return Ok(employee);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany( int companyId , [FromBody] EmployeeForCreationDto employee)
        {
            var employeeResult = await _serviceManager.employeeService.CreateEmployeeForCompany(companyId, employee, false);
            return CreatedAtRoute("GetEmployeeForCompany",new {companyId , employeeResult.Id},employeeResult);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(int companyId, int id)
        {
           await _serviceManager.employeeService.DeleteEmployeeForCompany(companyId, id, false);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(int companyId, int id, [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee is null)
                return BadRequest("EmployeeForUpdateDto object is null");
             await _serviceManager.employeeService.UpdateEmployeeForCompany(companyId, id, employee, false, true);
            return NoContent();
        }
    }
}
