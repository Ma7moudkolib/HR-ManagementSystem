using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        public EmployeeService(IRepositoryManager repositoryManager , ILoggerManager loggerManager,IMapper mapper)
        {
            _loggerManager = loggerManager;
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> CreateEmployeeForCompany(int companyId, EmployeeForCreationDto employee, bool trackChanges)
        {
            var company = await _repositoryManager.Company.GetCompany(companyId,trackChanges);
            if(company == null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = _mapper.Map<Employee>(company);
            _repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repositoryManager.savechanges();
            var employeeDto = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeDto;
        }

        public async Task DeleteEmployeeForCompany(int companyId, int employeeId, bool trackChange)
        {
            var company = await _repositoryManager.Company.GetCompany(companyId, trackChange);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employee =await _repositoryManager.Employee.GetEmployee(companyId, employeeId, trackChange);
            if (employee is null)
                throw new EmployeeNotFoundException(employeeId);
            _repositoryManager.Employee.DeleteEmployee(employee);
            await _repositoryManager.savechanges();
        }

        public async Task<EmployeeDto> GetEmployee(int companyId, int id, bool trackChanges)
        {
            var company = await _repositoryManager.Company.GetCompany(companyId, trackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyId);
            var employee =await _repositoryManager.Employee.GetEmployee(companyId,id , trackChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(companyId);
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployees(int companyId, bool trackChanges)
        {
           var company = await _repositoryManager.Company.GetCompany(companyId,trackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyId);
            var employees = await _repositoryManager.Employee.GetEmployees(companyId, trackChanges);
            var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return employeeDto;
        }

        public async Task UpdateEmployeeForCompany(int companyId, int id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
        {
           var company =await _repositoryManager.Company.GetCompany(companyId, compTrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity =await _repositoryManager.Employee.GetEmployee(companyId, id, empTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(id);
            _mapper.Map(employeeForUpdate, employeeEntity);
           await _repositoryManager.savechanges();
        }
    }
}
