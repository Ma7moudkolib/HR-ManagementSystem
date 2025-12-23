using Entities.Enums;
using Entities.Models;

namespace Contracts
{
    public interface ILeaveRequestRepository
    {
        void CreateLeaveRequestAsync(LeaveRequest request);
        void DeleteLeaveRequest(LeaveRequest request);
        Task<LeaveRequest?> GetLeaveRequestByIdAsync(int id,bool trackChanges);
        Task<IEnumerable<LeaveRequest>> GetByEmployeeAsync(int employeeId, bool trackChange);
        Task<IEnumerable<LeaveRequest>> GetByStatusAsync(LeaveStatus status,bool trackChange);
    }
}
