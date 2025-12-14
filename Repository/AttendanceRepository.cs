using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;

namespace Repository
{
    public class AttendanceRepository : RepositoryBase<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void CreateAttendance(Attendance attendance) => Create(attendance);

        public void DeleteAttendance(Attendance attendance) => Delete(attendance);

        public async Task<IEnumerable<Attendance?>> GetAllAttendancesAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();

        public async Task<IEnumerable<Attendance?>> GetAttendanceByEmployeeIdAsync(Guid employeeId, bool trackChanges) =>
           await FindByCondition(attendance => attendance.EmployeeId.Equals(employeeId), trackChanges).ToListAsync();

        public Task<Attendance?> GetAttendanceByEmployeeIdForDayAsync(Guid employeeId, DateTime date, bool trackChanges) =>
           FindByCondition(attendance => attendance.EmployeeId.Equals(employeeId)
            && attendance.CheckIn.Date == date.Date, trackChanges).FirstOrDefaultAsync();

        public async Task<IEnumerable<Attendance?>> GetAttendanceByEmployeeIdForMonthAsync(Guid employeeId, int month, int year, bool trackChanges) =>
           await FindByCondition(attendance => attendance.EmployeeId.Equals(employeeId)
            && attendance.CheckIn.Month == month
            && attendance.CheckIn.Year == year, trackChanges)
            .ToListAsync();

        public async Task<Attendance?> GetAttendancesByDateAsync(DateTime date, bool trackChanges) =>
           await FindByCondition(attendance => attendance.CheckIn.Date == date.Date, trackChanges).FirstOrDefaultAsync();

        public async Task<IEnumerable<Attendance>> GetEmployeesWithoutCheckoutAsync(DateTime date) =>
           await FindByCondition(attendance => attendance.CheckIn.Date == date.Date && attendance.CheckOut == null, false)
            .ToListAsync();
           
    }
}
