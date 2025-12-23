using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync(int companyId, bool trackChanges);
        Task<DepartmentDto> GetDepartmentAsync(int companyId, int departmentId, bool trackChanges);
        Task<DepartmentDto> CreateDepartmentForCompanyAsync(int companyId, DepartmentCreateDto departmentForCreation);
        Task DeleteDepartmentForCompanyAsync(int companyId, int departmentId, bool trackChanges);
        Task UpdateDepartmentForCompanyAsync(int companyId, int departmentId, DepartmentUpdateDto departmentForUpdate, bool trackChanges);
    }
}
