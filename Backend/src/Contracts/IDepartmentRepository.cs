using Entities.Models;
namespace Contracts
{
    public interface IDepartmentRepository
    {
        void CreateDepartment(int companyId,Department department);
        void UpdateDepartment(Department department);

        void DeleteDepartment(Department department);

        Task<IEnumerable<Department?>> GetAllDepartments(int companyId,bool trackChanges);
        Task<Department?> GetDepartmentById(int companyId ,int id , bool trackChanges);
    }
}
