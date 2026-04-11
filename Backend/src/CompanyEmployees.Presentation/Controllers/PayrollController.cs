using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;
        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePayroll([FromBody] Shared.DataTransferObjects.GeneratePayrollRequestDto payrollRequestDto)
        {
            var payrollPeriod = await _payrollService.GeneratePayrollAsync(payrollRequestDto);
            return Ok(payrollPeriod);
        }

        [HttpGet("company/{companyId}/year/{year}/month/{month}")]
        public async Task<IActionResult> GetPayrollByMonth(int companyId, int year, int month)
        {
            var payrollPeriod = await _payrollService.GetPayrollByMonthAsync(companyId, year, month);
            return Ok(payrollPeriod);
        }
        [HttpGet("employee/{employeeId}/year/{year}/month/{month}")]
        public async Task<IActionResult> GetEmployeePayroll(int companyId, int employeeId, int year, int month)
        {
            var payrollRecord = await _payrollService.GetEmployeePayrollAsync(companyId, employeeId, year, month);
            return Ok(payrollRecord);
        }
        [HttpPost("{payrollPeriodId}/close")]
        public async Task<IActionResult> ClosePayroll(int payrollPeriodId, [FromQuery] string closedBy)
        {
            var closedPayrollPeriod = await _payrollService.ClosePayrollAsync(payrollPeriodId, closedBy);
            return Ok(closedPayrollPeriod);
        }
    }
}
