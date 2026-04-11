using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/leave")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class LeaveController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public LeaveController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("status/{employeeId}")]
        public async Task<IActionResult> GetEmployeeLeaves(int employeeId)
        {
            var leaves = await _serviceManager.leaveService.GetEmployeeLeavesAsync(employeeId);
            return Ok(leaves);
        }
        [HttpGet("balances/{employeeId}")]
        public async Task<IActionResult> GetEmployeeLeaveBalances(int employeeId)
        {
            var balances = await _serviceManager.leaveService.GetEmployeeLeaveBalancesAsync(employeeId);
            return Ok(balances);
        }
        [HttpPost]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestDto leaveRequestDto)
        {
            var result = await _serviceManager.leaveService.CreateLeaveRequestAsync(leaveRequestDto);
            if (!result.Success)
                return BadRequest(result.message);
            return Ok(result.message);
        }
        [HttpPost("approve/{leaveRequestId}")]
        public async Task<IActionResult> ApproveLeaveRequest(int leaveRequestId)
        {
            var result = await _serviceManager.leaveService.ApproveLeaveAsync(leaveRequestId);
            if (!result.Success)
                return BadRequest(result.message);
            return Ok(result.message);
        }
        [HttpPost("reject/{leaveRequestId}")]
        public async Task<IActionResult> RejectLeaveRequest(int leaveRequestId)
        {
            var result = await _serviceManager.leaveService.RejectLeaveAsync(leaveRequestId);
            if (!result.Success)
                return BadRequest(result.message);
            return Ok(result.message);
        }
    }
}
