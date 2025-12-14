using Entities.Models;

namespace Contracts
{
    public interface ICompanyRepository 
    {
       Task<IEnumerable<Company?>> GetAllCompanies(bool trackChanges);
        Task<Company?> GetCompany(Guid companyId, bool trackChange);
        void CreateCompany(Company company);
        void DeleteCompany(Company company );
    }
}
