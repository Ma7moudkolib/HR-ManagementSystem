using Entities.Models;

namespace Contracts
{
    public interface IPayrollPeriodRepository
    {
        Task<PayrollPeriod?> GetByIdAsync(int id);
        Task<PayrollPeriod?> GetByCompanyAndMonthAsync(int companyId, int year, int month);
        Task<IEnumerable<PayrollPeriod>> GetByCompanyAsync(int companyId);
        void AddAsync(PayrollPeriod payrollPeriod);
        void UpdateAsync(PayrollPeriod payrollPeriod);
        Task<bool> ExistsAsync(int companyId, int year, int month);
    }
}
