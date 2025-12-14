using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public AttendanceService(IRepositoryManager repository , IMapper mapper )
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse> Checkin(Guid employeeId)
        {
           var employeeAttendance = await _repository.Attendance.GetAttendanceByEmployeeIdAsync(employeeId, false);
            if (employeeAttendance != null)
            {
                return new ServiceResponse(false,"Employee already checked in");
            }
            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                CheckIn = DateTime.UtcNow
            };
            _repository.Attendance.CreateAttendance(attendance);
            await _repository.savechanges();
            return new ServiceResponse(true, "Check-in successful");
        }

        public async Task<ServiceResponse> Checkout(Guid employeeId)
        {
            var employeeAttendance = await _repository.Attendance.GetAttendanceByEmployeeIdForDayAsync(employeeId,DateTime.UtcNow, true);
            if (employeeAttendance == null)
            {
                return new ServiceResponse(false,"Employee has not checked in");
            }
            employeeAttendance.WorkedHours = (decimal)(DateTime.UtcNow - employeeAttendance.CheckIn).TotalHours;
            employeeAttendance.CheckOut = DateTime.UtcNow;
            await _repository.savechanges();
            return new ServiceResponse(true, "Check-out successful");
        }

        public async Task<IEnumerable<AttendanceDto>> GetEmployeeAttendance(Guid employeeId)
        {
           var attendances =await _repository.Attendance.GetAttendanceByEmployeeIdAsync(employeeId, false);
            if (attendances == null)
            {
                throw new Exception("No attendance records found for the employee.");
            }
            var attendancesResult = _mapper.Map<IEnumerable<AttendanceDto>>(attendances);
            return attendancesResult; 
        }
    }
}
