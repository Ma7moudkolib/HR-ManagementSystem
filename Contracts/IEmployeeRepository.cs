using Entities.Models;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees(int companyId, bool trackChanges);
        Task<Employee?> GetEmployee(int companyId,int employeeId, bool trackChanges);
        void CreateEmployeeForCompany(int companyId,Employee employee);
        void DeleteEmployee(Employee employee);
        void UpdateEmployeeForCompany(Employee employee);
    }
}
