using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateEmployeeForCompany(int companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee)
            => Delete(employee);

        public async Task<Employee?> GetEmployee(int companyId, int employeeId, bool trackChanges) =>
          await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId)
            , trackChanges).SingleOrDefaultAsync();
      

        public async Task<IEnumerable<Employee>> GetEmployees(int companyId, bool trackChanges)=>
           await FindByCondition(c => c.CompanyId.Equals(companyId), trackChanges)
            .OrderBy(e=>e.Name).ToListAsync();

        public void UpdateEmployeeForCompany(Employee employee) 
            => Update(employee);
    }
}
