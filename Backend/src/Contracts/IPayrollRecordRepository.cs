using Entities.Models;

namespace Contracts
{
    public interface IPayrollRecordRepository
    {
        Task<PayrollRecord?> GetByIdAsync(int id);
        Task<IEnumerable<PayrollRecord>> GetByPayrollPeriodAsync(int payrollPeriodId);
        Task<PayrollRecord?> GetByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId);
        Task<IEnumerable<PayrollRecord>> GetByEmployeeAsync(int employeeId, int year, int month);
        void AddRangeAsync(IEnumerable<PayrollRecord> payrollRecords);
        Task<int> CountByPayrollPeriodAsync(int payrollPeriodId);
    }
}
