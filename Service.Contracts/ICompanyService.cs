using Shared.DataTransferObjects;
namespace Service.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompanies(bool trackChanges);
        Task<CompanyDto> GetCompany(int companyId , bool trackChanges);
        Task<CompanyDto> CreateCompany(CompanyForCreationDto company);
        Task DeleteCompany(int companyId,bool trackChanges);
        Task UpdateCompany(CompanyDto company);

    }
}
