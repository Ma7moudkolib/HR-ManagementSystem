using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;

namespace Repository
{
    public class LeaveBalanceRepository : RepositoryBase<LeaveBalance>, ILeaveBalanceRepository
    {
        public LeaveBalanceRepository(RepositoryContext repositoryContext): base(repositoryContext) { }
        public async Task<LeaveBalance?> GetBalanceAsync(int employeeId, int leaveTypeId, bool trackChange)
            =>await FindByCondition(lb => lb.EmployeeId == employeeId && lb.LeaveTypeId == leaveTypeId, trackChange)
            .FirstOrDefaultAsync();


        public async Task<IEnumerable<LeaveBalance>> GetBalancesForEmployeeAsync(int employeeId, bool trackChange) => await
            FindByCondition(lb => lb.EmployeeId == employeeId, trackChange)
            .ToListAsync();
    }

}
