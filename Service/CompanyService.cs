using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class CompanyService :ICompanyService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        public CompanyService(IRepositoryManager repositoryManager , ILoggerManager loggerManager,IMapper mapper)
        {
            _loggerManager = loggerManager;
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<CompanyDto> GetCompany(int companyId, bool trackChanges)
        {
            var company = await _repositoryManager.Company.GetCompany(companyId,trackChanges);
            if(company is  null)
                throw new CompanyNotFoundException(companyId);
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompanies(bool trackChanges)
        {
            var companies =await _repositoryManager.Company.GetAllCompanies(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
        }

        public async Task<CompanyDto> CreateCompany(CompanyForCreationDto company)
        {
            var comapnyEntity = _mapper.Map<Company>(company);
            _repositoryManager.Company.CreateCompany(comapnyEntity);
           await _repositoryManager.savechanges();
            var companyDto = _mapper.Map<CompanyDto>(comapnyEntity);
            return companyDto;
        }

        public async Task DeleteCompany(int companyId, bool trackChanges)
        {
            var company = await _repositoryManager.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            _repositoryManager.Company.DeleteCompany(company);
            await _repositoryManager.savechanges();
        }

        public async Task UpdateCompany(CompanyDto company)
        {
            var companyEntity = await _repositoryManager.Company.GetCompany(company.Id, true);
            if (companyEntity is null)
                throw new CompanyNotFoundException(company.Id);
            _mapper.Map(company, companyEntity);
            await _repositoryManager.savechanges();
        }
    }
}
