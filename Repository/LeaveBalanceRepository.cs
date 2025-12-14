using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;

namespace Repository
{
    public class LeaveBalanceRepository : RepositoryBase<LeaveBalance>, ILeaveBalanceRepository
    {
        public LeaveBalanceRepository(RepositoryContext repositoryContext): base(repositoryContext) { }
        public async Task<LeaveBalance?> GetBalanceAsync(Guid employeeId, Guid leaveTypeId, bool trackChange)
            =>await FindByCondition(lb => lb.EmployeeId == employeeId && lb.LeaveTypeId == leaveTypeId, trackChange)
            .FirstOrDefaultAsync();


        public async Task<IEnumerable<LeaveBalance>> GetBalancesForEmployeeAsync(Guid employeeId, bool trackChange) => await
            FindByCondition(lb => lb.EmployeeId == employeeId, trackChange)
            .ToListAsync();
    }

}
