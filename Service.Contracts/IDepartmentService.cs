using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync(Guid companyId, bool trackChanges);
        Task<DepartmentDto> GetDepartmentAsync(Guid companyId, Guid departmentId, bool trackChanges);
        Task<DepartmentDto> CreateDepartmentForCompanyAsync(Guid companyId, DepartmentCreateDto departmentForCreation);
        Task DeleteDepartmentForCompanyAsync(Guid companyId, Guid departmentId, bool trackChanges);
        Task UpdateDepartmentForCompanyAsync(Guid companyId, Guid departmentId, DepartmentUpdateDto departmentForUpdate, bool trackChanges);
    }
}
