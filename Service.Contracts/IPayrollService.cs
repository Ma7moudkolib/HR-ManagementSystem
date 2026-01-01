using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IPayrollService
    {
        Task<PayrollPeriodDto> GeneratePayrollAsync(GeneratePayrollRequestDto payrollRequestDto);
        Task<PayrollPeriodDto> GetPayrollByMonthAsync(int companyId,int year, int month);
        Task<PayrollRecordDto> GetEmployeePayrollAsync(int companyId, int employeeId, int year, int month);
        Task<PayrollPeriodDto> ClosePayrollAsync(int payrollPeriodId, string closedBy);
    }
}
