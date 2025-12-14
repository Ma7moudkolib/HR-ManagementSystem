using AutoMapper;
using Contracts;
using Entities.Enums;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class LeaveService : ILeaveService 
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        public LeaveService(IRepositoryManager repositoryManager,IMapper mapper )
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> ApproveLeaveAsync(Guid leaveRequestId)
        {
            var request =await _repositoryManager.LeaveRequest.GetLeaveRequestByIdAsync(leaveRequestId, true);
            if (request is null)
                return new ServiceResponse(false, "Leave request not found");
            if (request.Status != LeaveStatus.Pending)
                return new ServiceResponse(false, "Leave already processed");
            
            var leaveType = await _repositoryManager.leaveTypeRepository.GetLeaveTypeByIdAsync(request.LeaveTypeId, false);
            if (leaveType.IsPaid)
            {
                var balance = await _repositoryManager.LeaveBalance.GetBalanceAsync(request.EmployeeId, request.LeaveTypeId, true);
                if (balance is null )
                    return new ServiceResponse(false, "Leave balance not found");
                balance.RemainingDays -= request.TotalDays;
            }
            request.Status = LeaveStatus.Approved;
            request.ActionDate = DateTime.UtcNow;
            await _repositoryManager.savechanges();
            return new ServiceResponse(true, "Leave approved successfully");

        }

        public async Task<ServiceResponse> CreateLeaveRequestAsync(CreateLeaveRequestDto leaveRequestDto)
        {
            if (leaveRequestDto.StartDate > leaveRequestDto.EndDate)
                return new ServiceResponse(false, "Invalid leave date range");
            var totalDays = (leaveRequestDto.EndDate - leaveRequestDto.StartDate).Days;

            var leaveType = await _repositoryManager.leaveTypeRepository.GetLeaveTypeByIdAsync(leaveRequestDto.LeaveTypeId,false);
            var balance = await _repositoryManager.LeaveBalance.GetBalanceAsync(leaveRequestDto.EmployeeId, leaveRequestDto.LeaveTypeId, false);

            if (leaveType.IsPaid)
            {
                if (balance is null || balance.RemainingDays < totalDays)
                    return new ServiceResponse(false, "Insufficient leave balance");
            }
            var request = _mapper.Map<LeaveRequest>(leaveRequestDto);
            request.TotalDays = totalDays;
             _repositoryManager.LeaveRequest.CreateLeaveRequestAsync(request);
            await _repositoryManager.savechanges();
            return new ServiceResponse(true, "Leave request created successfully");
        }

        public async Task<IEnumerable<LeaveBalanceDto>> GetEmployeeLeaveBalancesAsync(Guid employeeId)
        {
           var balances =await _repositoryManager.LeaveBalance.GetBalancesForEmployeeAsync(employeeId, false);
            return _mapper.Map<IEnumerable<LeaveBalanceDto>>(balances);
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetEmployeeLeavesAsync(Guid employeeId)
        {
            var leaves =await _repositoryManager.LeaveRequest.GetByEmployeeAsync(employeeId, false);
            return _mapper.Map<IEnumerable<LeaveRequestDto>>(leaves);
        }

        public async Task<ServiceResponse> RejectLeaveAsync(Guid leaveRequestId)
        {
            var request = await _repositoryManager.LeaveRequest.GetLeaveRequestByIdAsync(leaveRequestId, true);
            if (request is null)
                return new ServiceResponse(false, "Leave request not found");
            if (request.Status != LeaveStatus.Pending)
                return new ServiceResponse(false, "Leave already processed");
            request.Status = LeaveStatus.Rejected;
            request.ActionDate = DateTime.UtcNow;
            await _repositoryManager.savechanges();
            return new ServiceResponse(true, "Leave rejected");
        }
    }
}
