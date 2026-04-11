namespace Contracts
{
    public interface IRepositoryManager
    {
        IEmployeeRepository Employee { get; }
        ICompanyRepository Company { get; }
        IDepartmentRepository Department { get; }
        IAttendanceRepository Attendance { get; }
        ILeaveBalanceRepository LeaveBalance { get; }
        ILeaveRequestRepository LeaveRequest { get; }
        ILeaveTypeRepository leaveTypeRepository { get; }
        IPayrollPeriodRepository payrollPeriodRepository { get; }
        IPayrollRecordRepository payrollRecordRepository { get; }
        Task savechanges();
    }
}
