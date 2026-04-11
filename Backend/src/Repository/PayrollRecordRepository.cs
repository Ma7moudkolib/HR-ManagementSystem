using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;

namespace Repository
{
    public class PayrollRecordRepository :IPayrollRecordRepository
    {
        private readonly RepositoryContext _context;

        public PayrollRecordRepository(RepositoryContext context)
        {
            _context = context;
        }

        public async Task<PayrollRecord?> GetByIdAsync(int id)
        {
            return await _context.PayrollRecords
                .Include(r => r.Employee)
                .Include(r => r.PayrollPeriod)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<PayrollRecord>> GetByPayrollPeriodAsync(int payrollPeriodId)
        {
            return await _context.PayrollRecords
                .Include(r => r.Employee)
                .Where(r => r.PayrollPeriodId == payrollPeriodId)
                .OrderBy(r => r.EmployeeName)
                .ToListAsync();
        }

        public async Task<PayrollRecord?> GetByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId)
        {
            return await _context.PayrollRecords
                .Include(r => r.Employee)
                .Include(r => r.PayrollPeriod)
                .FirstOrDefaultAsync(r => r.EmployeeId == employeeId
                    && r.PayrollPeriodId == payrollPeriodId);
        }

        public async Task<IEnumerable<PayrollRecord>> GetByEmployeeAsync(int employeeId, int year, int month)
        {
            return await _context.PayrollRecords
                .Include(r => r.PayrollPeriod)
                .Where(r => r.EmployeeId == employeeId
                    && r.PayrollPeriod.Year == year
                    && r.PayrollPeriod.Month == month)
                .ToListAsync();
        }

        public  void AddRangeAsync(IEnumerable<PayrollRecord> payrollRecords)
        {
             _context.PayrollRecords.AddRangeAsync(payrollRecords);
        }

        public async Task<int> CountByPayrollPeriodAsync(int payrollPeriodId)
        {
            return await _context.PayrollRecords
                .CountAsync(r => r.PayrollPeriodId == payrollPeriodId);
        }
    }
}
