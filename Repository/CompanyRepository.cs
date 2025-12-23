using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateCompany(Company company)=>Create(company);

        public void DeleteCompany(Company company) => Delete(company);

        public async Task<IEnumerable<Company?>> GetAllCompanies(bool trackChanges)=>
            await FindAll(trackChanges)
            .OrderBy(c=>c.Name)
            .ToListAsync();

        public async Task<Company?> GetCompany(int companyId, bool trackChange) =>
          await FindByCondition(c => c.Id.Equals(companyId), trackChange).SingleOrDefaultAsync();
            
    }
}
