using Entities.Models;

namespace Contracts
{
    public interface ILeaveBalanceRepository
    {
        Task<LeaveBalance?> GetBalanceAsync(Guid employeeId, Guid leaveTypeId,bool trackChange);
        Task<IEnumerable<LeaveBalance>> GetBalancesForEmployeeAsync(Guid employeeId, bool trackChange);
    }
}
