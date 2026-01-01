using Contracts;
using Repository.DatabaseContext;

namespace Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<ICompanyRepository> _companyRepository;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;
        private readonly Lazy<IDepartmentRepository> _departmentRepository;
        private readonly Lazy<IAttendanceRepository> _attendanceRepository;
        private readonly Lazy<ILeaveBalanceRepository> _leaveBalanceRepository;
        private readonly Lazy<ILeaveRequestRepository> _leaveRequestRepository;
        private readonly Lazy<ILeaveTypeRepository> _leaveTypeRepository;
        private readonly Lazy<IPayrollPeriodRepository> _payrollPeriodRepository;
        private readonly Lazy<IPayrollRecordRepository> _payrollRecordRepository;
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(repositoryContext));
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(repositoryContext));
            _departmentRepository = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(repositoryContext));
            _attendanceRepository = new Lazy<IAttendanceRepository>(() => new AttendanceRepository(repositoryContext));
            _leaveBalanceRepository = new Lazy<ILeaveBalanceRepository>(() => new LeaveBalanceRepository(repositoryContext));
            _leaveRequestRepository = new Lazy<ILeaveRequestRepository>(() => new LeaveRequestRepository(repositoryContext));
            _leaveTypeRepository = new Lazy<ILeaveTypeRepository>(() => new LeaveTypeRepository(repositoryContext));
            _payrollPeriodRepository = new Lazy<IPayrollPeriodRepository>(() => new PayrollPeriodRepository(repositoryContext));
            _payrollRecordRepository = new Lazy<IPayrollRecordRepository>(() => new PayrollRecordRepository(repositoryContext));
        }
        public ICompanyRepository Company => _companyRepository.Value;
        public IEmployeeRepository Employee => _employeeRepository.Value;
        public IDepartmentRepository Department => _departmentRepository.Value;
        public IAttendanceRepository Attendance => _attendanceRepository.Value;
        public ILeaveBalanceRepository LeaveBalance => _leaveBalanceRepository.Value;
        public ILeaveRequestRepository LeaveRequest => _leaveRequestRepository.Value;
        public ILeaveTypeRepository leaveTypeRepository => _leaveTypeRepository.Value;
        public IPayrollPeriodRepository payrollPeriodRepository => _payrollPeriodRepository.Value;
        public IPayrollRecordRepository payrollRecordRepository => _payrollRecordRepository.Value;

        public async Task savechanges()=> await _repositoryContext.SaveChangesAsync();
    }
}
