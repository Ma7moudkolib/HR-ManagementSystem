using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/attendance")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AttendanceController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public AttendanceController(IServiceManager serviceManager) => _serviceManager = serviceManager;
        [HttpPost("checkin/{employeeId}")]
        public async Task<IActionResult> Checkin(int employeeId)
        {
            var result = await _serviceManager.attendanceService.Checkin(employeeId);
            return Ok(result);
        }
        [HttpPost("checkout/{employeeId}")]
        public async Task<IActionResult> Checkout(int employeeId)
        {
            var result = await _serviceManager.attendanceService.Checkout(employeeId);
            return Ok(result);
        }
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetEmployeeAttendance(int employeeId)
        {
            var result = await _serviceManager.attendanceService.GetEmployeeAttendance(employeeId);
            return Ok(result);
        }
    }
}
