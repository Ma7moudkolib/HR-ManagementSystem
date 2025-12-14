using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IAttendanceService
    {
        public Task<ServiceResponse> Checkin(Guid employeeId);
        public Task<ServiceResponse> Checkout(Guid employeeId);
        public Task<IEnumerable<AttendanceDto>> GetEmployeeAttendance(Guid employeeId);

    }
}
