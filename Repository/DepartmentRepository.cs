using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;
using System.ComponentModel.Design;

namespace Repository
{
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public void CreateDepartment(Guid comapnyId,Department department) { 
            department.CompanyId = comapnyId;
            Create(department); 
        }

        public void DeleteDepartment(Department department)=> Delete(department);
  
        public async Task<IEnumerable<Department?>> GetAllDepartments(Guid companyId, bool trackChanges)
            => await FindByCondition(c => c.CompanyId.Equals(companyId), trackChanges)
            .OrderBy(e => e.Name).ToListAsync();

        public async Task<Department?> GetDepartmentById(Guid companyId,Guid id, bool trackChanges)
           => await FindByCondition(d => d.CompanyId.Equals(companyId) && d.Id.Equals(id)
            , trackChanges).SingleOrDefaultAsync();

        public void UpdateDepartment(Department department) => Update(department);
    }
}
