using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ILeaveService
    {
        Task<ServiceResponse>  CreateLeaveRequestAsync(CreateLeaveRequestDto leaveRequestDto);
        Task<ServiceResponse> ApproveLeaveAsync(Guid leaveRequestId);
        Task<ServiceResponse> RejectLeaveAsync(Guid leaveRequestId);
        Task<IEnumerable<LeaveRequestDto>> GetEmployeeLeavesAsync(Guid employeeId);
        Task<IEnumerable<LeaveBalanceDto>> GetEmployeeLeaveBalancesAsync(Guid employeeId);

    }
}
