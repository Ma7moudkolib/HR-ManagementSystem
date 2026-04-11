using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ILeaveService
    {
        Task<ServiceResponse>  CreateLeaveRequestAsync(CreateLeaveRequestDto leaveRequestDto);
        Task<ServiceResponse> ApproveLeaveAsync(int leaveRequestId);
        Task<ServiceResponse> RejectLeaveAsync(int leaveRequestId);
        Task<IEnumerable<LeaveRequestDto>> GetEmployeeLeavesAsync(int employeeId);
        Task<IEnumerable<LeaveBalanceDto>> GetEmployeeLeaveBalancesAsync(int employeeId);

    }
}
