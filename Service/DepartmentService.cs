using AutoMapper;
using Contracts;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        public DepartmentService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync(Guid companyId, bool trackChanges)
        {
           var company = await _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new KeyNotFoundException($"Company with id: {companyId} doesn't exist in the database.");
            var departmentsFromDb = await _repository.Department.GetAllDepartments(companyId, trackChanges);
            var departmentsDto = _mapper.Map<IEnumerable<DepartmentDto>>(departmentsFromDb);
            return departmentsDto;
        }
        public async Task<DepartmentDto> GetDepartmentAsync(Guid companyId, Guid departmentId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new KeyNotFoundException($"Company with id: {companyId} doesn't exist in the database.");
            var departmentDb =await _repository.Department.GetDepartmentById(companyId, departmentId, trackChanges);
            if (departmentDb is null)
                throw new KeyNotFoundException($"Department with id: {departmentId} doesn't exist in the database.");
            var departmentDto = _mapper.Map<DepartmentDto>(departmentDb);
            return departmentDto;
        }
        public async Task<DepartmentDto> CreateDepartmentForCompanyAsync(Guid companyId, DepartmentCreateDto departmentForCreation)
        {
            var company = await _repository.Company.GetCompany(companyId, false);
            if (company is null)
                throw new KeyNotFoundException($"Company with id: {companyId} doesn't exist in the database.");
            var departmentEntity = _mapper.Map<Entities.Models.Department>(departmentForCreation);
            _repository.Department.CreateDepartment(companyId, departmentEntity);
            await _repository.savechanges();
            var departmentToReturn = _mapper.Map<DepartmentDto>(departmentEntity);
            return departmentToReturn;
        }
        public async Task DeleteDepartmentForCompanyAsync(Guid companyId, Guid departmentId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new KeyNotFoundException($"Company with id: {companyId} doesn't exist in the database.");
            var departmentDb = await _repository.Department.GetDepartmentById(companyId, departmentId, trackChanges);
            if (departmentDb is null)
                throw new KeyNotFoundException($"Department with id: {departmentId} doesn't exist in the database.");
            _repository.Department.DeleteDepartment(departmentDb);
            await _repository.savechanges();
        }
        public async Task UpdateDepartmentForCompanyAsync(Guid companyId, Guid departmentId, DepartmentUpdateDto departmentForUpdate, bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new KeyNotFoundException($"Company with id: {companyId} doesn't exist in the database.");
            var departmentDb = await _repository.Department.GetDepartmentById(companyId, departmentId, trackChanges);
            if (departmentDb is null)
                throw new KeyNotFoundException($"Department with id: {departmentId} doesn't exist in the database.");
            _mapper.Map(departmentForUpdate, departmentDb);
            await _repository.savechanges();
        }
        
    }
}
