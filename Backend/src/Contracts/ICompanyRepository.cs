using Entities.Models;

namespace Contracts
{
    public interface ICompanyRepository 
    {
       Task<IEnumerable<Company?>> GetAllCompanies(bool trackChanges);
        Task<Company?> GetCompany(int companyId, bool trackChange);
        void CreateCompany(Company company);
        void DeleteCompany(Company company );
    }
}
