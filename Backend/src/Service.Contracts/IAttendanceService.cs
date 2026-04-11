using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IAttendanceService
    {
        public Task<ServiceResponse> Checkin(int employeeId);
        public Task<ServiceResponse> Checkout(int employeeId);
        public Task<IEnumerable<AttendanceDto>> GetEmployeeAttendance(int employeeId);

    }
}
