using Entities.Models;

namespace Contracts
{
    public interface ILeaveTypeRepository
    {
        Task<IEnumerable<LeaveType>> GetAllLeaveTypeAsync(bool trackChange);
        Task<LeaveType> GetLeaveTypeByIdAsync(int id,bool trackChange);
        void CreateLeaveType(LeaveType leaveType);
        void DeleteLeaveType(LeaveType leaveType);
    }
}
