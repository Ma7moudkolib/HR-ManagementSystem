namespace Service.Contracts
{
    public interface IServiceManager
    {
        ICompanyService companyService { get; }
        IEmployeeService employeeService { get; }
        IAuthenticationService authenticationService { get; }
        IDepartmentService departmentService { get; }
        IAttendanceService attendanceService { get; }
        ILeaveService leaveService { get; }
        IPayrollService payrollService { get; }
    }
}
