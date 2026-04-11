using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;

namespace Service
{
    public sealed class ServiceManager :IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IDepartmentService> _departmentService;
        private readonly Lazy<IAttendanceService> _attendanceService;
        private readonly Lazy<ILeaveService> _leaveService;
        private readonly Lazy<IPayrollService> _payrollService;
        public ServiceManager(IRepositoryManager repositoryManager , ILoggerManager loggerManager,IMapper mapper,UserManager<User> userManager,IConfiguration configuration  )
        {
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, loggerManager,mapper));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager,loggerManager,mapper));
            _departmentService = new Lazy<IDepartmentService>(() => new DepartmentService(repositoryManager, loggerManager, mapper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(loggerManager, mapper,userManager,configuration));
            _attendanceService = new Lazy<IAttendanceService>(() => new AttendanceService(repositoryManager, mapper));
            _leaveService = new Lazy<ILeaveService>(() => new LeaveService(repositoryManager, mapper));
            _payrollService = new Lazy<IPayrollService>(() => new PayrollService(repositoryManager, mapper));
        }

        public ICompanyService companyService => _companyService.Value;
        public IDepartmentService departmentService => _departmentService.Value;
        public IEmployeeService employeeService => _employeeService.Value;
        public IAuthenticationService authenticationService => _authenticationService.Value;
        public IAttendanceService attendanceService => _attendanceService.Value;
        public ILeaveService leaveService => _leaveService.Value;
        public IPayrollService payrollService => _payrollService.Value;
    }
}
