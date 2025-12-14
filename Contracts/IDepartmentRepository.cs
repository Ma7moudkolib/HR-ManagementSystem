using Entities.Models;
namespace Contracts
{
    public interface IDepartmentRepository
    {
        void CreateDepartment(Guid companyId,Department department);
        void UpdateDepartment(Department department);

        void DeleteDepartment(Department department);

        Task<IEnumerable<Department?>> GetAllDepartments(Guid companyId,bool trackChanges);
        Task<Department?> GetDepartmentById(Guid companyId ,Guid id , bool trackChanges);
    }
}
