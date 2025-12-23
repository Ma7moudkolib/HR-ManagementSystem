using Entities.Models;

namespace Contracts
{
    public interface ILeaveBalanceRepository
    {
        Task<LeaveBalance?> GetBalanceAsync(int employeeId, int leaveTypeId,bool trackChange);
        Task<IEnumerable<LeaveBalance>> GetBalancesForEmployeeAsync(int employeeId, bool trackChange);
    }
}
