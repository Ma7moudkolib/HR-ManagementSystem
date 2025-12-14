using Entities.Models;

namespace Contracts
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance?>> GetAllAttendancesAsync(bool trackChanges);
        Task<IEnumerable<Attendance?>> GetAttendanceByEmployeeIdAsync(Guid employeeId, bool trackChanges);
        void CreateAttendance(Attendance attendance);
        void DeleteAttendance(Attendance attendance);
        Task<IEnumerable<Attendance?>> GetAttendanceByEmployeeIdForMonthAsync(Guid employeeId, int month, int year, bool trackChanges);
        Task<Attendance?> GetAttendanceByEmployeeIdForDayAsync(Guid employeeId, DateTime date, bool trackChanges);
        Task<IEnumerable<Attendance>> GetEmployeesWithoutCheckoutAsync(DateTime date);
        Task<Attendance?> GetAttendancesByDateAsync(DateTime date, bool trackChanges);

    }
}
