using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetEmployees(int companyId, bool trackChanges);
        Task<EmployeeDto> GetEmployee(int companyId,int id , bool trackChanges);
        Task<EmployeeDto> CreateEmployeeForCompany(int companyId,EmployeeForCreationDto employee, bool trackChanges);
        Task DeleteEmployeeForCompany(int companyId ,int employeeId,bool trackChange);
        Task UpdateEmployeeForCompany(int companyId, int id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);
    }
}
