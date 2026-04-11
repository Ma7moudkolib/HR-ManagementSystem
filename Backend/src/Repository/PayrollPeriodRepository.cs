using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;

namespace Repository
{
    public class PayrollPeriodRepository : IPayrollPeriodRepository
    {
        private readonly RepositoryContext _context;

        public PayrollPeriodRepository(RepositoryContext context)
        {
            _context = context;
        }

        public async Task<PayrollPeriod?> GetByIdAsync(int id)
        {
            return await _context.PayrollPeriods
                .Include(p => p.Company)
                .Include(p => p.PayrollRecords)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PayrollPeriod?> GetByCompanyAndMonthAsync(int companyId, int year, int month)
        {
            return await _context.PayrollPeriods
                .Include(p => p.Company)
                .Include(p => p.PayrollRecords)
                .FirstOrDefaultAsync(p => p.CompanyId == companyId
                    && p.Year == year
                    && p.Month == month);
        }

        public async Task<IEnumerable<PayrollPeriod>> GetByCompanyAsync(int companyId)
        {
            return await _context.PayrollPeriods
                .Include(p => p.Company)
                .Where(p => p.CompanyId == companyId)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();
        }

        public  void AddAsync(PayrollPeriod payrollPeriod)
        {
            _context.PayrollPeriods.AddAsync(payrollPeriod);
        }

        public void UpdateAsync(PayrollPeriod payrollPeriod)
        {
            _context.PayrollPeriods.Update(payrollPeriod);
        }

        public async Task<bool> ExistsAsync(int companyId, int year, int month)
        {
            return await _context.PayrollPeriods
                .AnyAsync(p => p.CompanyId == companyId
                    && p.Year == year
                    && p.Month == month);
        }
    }
}
