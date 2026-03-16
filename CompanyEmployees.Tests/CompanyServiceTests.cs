using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Moq;
using Service;
using Service.Contracts;
using Shared.DataTransferObjects;
using Xunit;

namespace CompanyEmployees.Tests
{
    public class CompanyServiceTests
    {
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly Mock<ILoggerManager> _mockLoggerManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ICompanyService _companyService;

        public CompanyServiceTests()
        {
            _mockRepositoryManager = new Mock<IRepositoryManager>();
            _mockLoggerManager = new Mock<ILoggerManager>();
            _mockMapper = new Mock<IMapper>();
            _companyService = new CompanyService(_mockRepositoryManager.Object, _mockLoggerManager.Object, _mockMapper.Object);
        }

        #region GetCompany Tests

        [Fact]
        public async Task GetCompany_WithValidCompanyId_ReturnsCompanyDto()
        {
            // Arrange
            int companyId = 1;
            var company = new Company
            {
                Id = companyId,
                Name = "Tech Corp",
                Address = "123 Main St",
                Country = "USA"
            };
            var companyDto = new CompanyDto(companyId, "Tech Corp", "123 Main St");

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyId, false))
                .ReturnsAsync(company);
            _mockMapper.Setup(x => x.Map<CompanyDto>(company))
                .Returns(companyDto);

            // Act
            var result = await _companyService.GetCompany(companyId, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(companyId, result.Id);
            Assert.Equal("Tech Corp", result.Name);
            Assert.Equal("123 Main St", result.FullAddress);
            _mockRepositoryManager.Verify(x => x.Company.GetCompany(companyId, false), Times.Once);
            _mockMapper.Verify(x => x.Map<CompanyDto>(company), Times.Once);
        }

        [Fact]
        public async Task GetCompany_WithInvalidCompanyId_ThrowsCompanyNotFoundException()
        {
            // Arrange
            int companyId = 999;
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyId, false))
                .ReturnsAsync((Company)null);

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() => 
                _companyService.GetCompany(companyId, false));
            _mockRepositoryManager.Verify(x => x.Company.GetCompany(companyId, false), Times.Once);
        }

        [Fact]
        public async Task GetCompany_WithTrackChangesTrue_PassesTrueToRepository()
        {
            // Arrange
            int companyId = 1;
            var company = new Company { Id = companyId, Name = "Test Corp" };
            var companyDto = new CompanyDto(companyId, "Test Corp", "Address");

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyId, true))
                .ReturnsAsync(company);
            _mockMapper.Setup(x => x.Map<CompanyDto>(company))
                .Returns(companyDto);

            // Act
            await _companyService.GetCompany(companyId, true);

            // Assert
            _mockRepositoryManager.Verify(x => x.Company.GetCompany(companyId, true), Times.Once);
        }

        #endregion

        #region GetAllCompanies Tests

        [Fact]
        public async Task GetAllCompanies_ReturnsAllCompanies()
        {
            // Arrange
            var companies = new List<Company>
            {
                new Company { Id = 1, Name = "Tech Corp", Address = "123 Main St", Country = "USA" },
                new Company { Id = 2, Name = "Finance Inc", Address = "456 Oak Ave", Country = "USA" },
                new Company { Id = 3, Name = "Global Ltd", Address = "789 Pine Rd", Country = "UK" }
            };

            var companiesDto = new List<CompanyDto>
            {
                new CompanyDto(1, "Tech Corp", "123 Main St"),
                new CompanyDto(2, "Finance Inc", "456 Oak Ave"),
                new CompanyDto(3, "Global Ltd", "789 Pine Rd")
            };

            _mockRepositoryManager.Setup(x => x.Company.GetAllCompanies(false))
                .ReturnsAsync(companies);
            _mockMapper.Setup(x => x.Map<IEnumerable<CompanyDto>>(companies))
                .Returns(companiesDto);

            // Act
            var result = await _companyService.GetAllCompanies(false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Collection(result,
                item => Assert.Equal("Tech Corp", item.Name),
                item => Assert.Equal("Finance Inc", item.Name),
                item => Assert.Equal("Global Ltd", item.Name));
            _mockRepositoryManager.Verify(x => x.Company.GetAllCompanies(false), Times.Once);
        }

        [Fact]
        public async Task GetAllCompanies_WithEmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            var companies = new List<Company>();
            var companiesDto = new List<CompanyDto>();

            _mockRepositoryManager.Setup(x => x.Company.GetAllCompanies(false))
                .ReturnsAsync(companies);
            _mockMapper.Setup(x => x.Map<IEnumerable<CompanyDto>>(companies))
                .Returns(companiesDto);

            // Act
            var result = await _companyService.GetAllCompanies(false);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepositoryManager.Verify(x => x.Company.GetAllCompanies(false), Times.Once);
        }

        [Fact]
        public async Task GetAllCompanies_WithTrackChangesTrue_PassesTrueToRepository()
        {
            // Arrange
            var companies = new List<Company>();
            var companiesDto = new List<CompanyDto>();

            _mockRepositoryManager.Setup(x => x.Company.GetAllCompanies(true))
                .ReturnsAsync(companies);
            _mockMapper.Setup(x => x.Map<IEnumerable<CompanyDto>>(companies))
                .Returns(companiesDto);

            // Act
            await _companyService.GetAllCompanies(true);

            // Assert
            _mockRepositoryManager.Verify(x => x.Company.GetAllCompanies(true), Times.Once);
        }

        #endregion

        #region CreateCompany Tests

        [Fact]
        public async Task CreateCompany_WithValidInput_CreatesAndReturnsCompanyDto()
        {
            // Arrange
            var companyForCreation = new CompanyForCreationDto("Tech Corp", "123 Main St", "USA");
            var companyEntity = new Company
            {
                Id = 1,
                Name = "Tech Corp",
                Address = "123 Main St",
                Country = "USA"
            };
            var companyDto = new CompanyDto(1, "Tech Corp", "123 Main St");

            _mockMapper.Setup(x => x.Map<Company>(companyForCreation))
                .Returns(companyEntity);
            _mockRepositoryManager.Setup(x => x.Company.CreateCompany(It.IsAny<Company>()))
                .Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<CompanyDto>(It.IsAny<Company>()))
                .Returns(companyDto);

            // Act
            var result = await _companyService.CreateCompany(companyForCreation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tech Corp", result.Name);
            Assert.Equal("123 Main St", result.FullAddress);
            _mockMapper.Verify(x => x.Map<Company>(companyForCreation), Times.Once);
            _mockRepositoryManager.Verify(x => x.Company.CreateCompany(It.IsAny<Company>()), Times.Once);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        [Fact]
        public async Task CreateCompany_MapsCorrectlyFromCreationDto()
        {
            // Arrange
            var companyForCreation = new CompanyForCreationDto("New Corp", "456 Oak Ave", "Canada");
            var companyEntity = new Company
            {
                Id = 2,
                Name = "New Corp",
                Address = "456 Oak Ave",
                Country = "Canada"
            };
            var companyDto = new CompanyDto(2, "New Corp", "456 Oak Ave");

            _mockMapper.Setup(x => x.Map<Company>(companyForCreation))
                .Returns(companyEntity);
            _mockRepositoryManager.Setup(x => x.Company.CreateCompany(It.IsAny<Company>()))
                .Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<CompanyDto>(It.IsAny<Company>()))
                .Returns(companyDto);

            // Act
            await _companyService.CreateCompany(companyForCreation);

            // Assert
            _mockRepositoryManager.Verify(x => 
                x.Company.CreateCompany(It.Is<Company>(c => 
                    c.Name == "New Corp" &&
                    c.Address == "456 Oak Ave" &&
                    c.Country == "Canada")), 
                Times.Once);
        }

        [Fact]
        public async Task CreateCompany_CallsSaveChanges()
        {
            // Arrange
            var companyForCreation = new CompanyForCreationDto("Save Test", "789 Elm St", "USA");
            var companyEntity = new Company { Id = 3, Name = "Save Test", Address = "789 Elm St", Country = "USA" };
            var companyDto = new CompanyDto(3, "Save Test", "789 Elm St");

            _mockMapper.Setup(x => x.Map<Company>(companyForCreation))
                .Returns(companyEntity);
            _mockRepositoryManager.Setup(x => x.Company.CreateCompany(It.IsAny<Company>()))
                .Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<CompanyDto>(It.IsAny<Company>()))
                .Returns(companyDto);

            // Act
            await _companyService.CreateCompany(companyForCreation);

            // Assert
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        #endregion

        #region DeleteCompany Tests

        [Fact]
        public async Task DeleteCompany_WithValidCompanyId_DeletesSuccessfully()
        {
            // Arrange
            int companyId = 1;
            var company = new Company { Id = companyId, Name = "Tech Corp", Address = "123 Main St" };

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyId, true))
                .ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Company.DeleteCompany(company))
                .Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);

            // Act
            await _companyService.DeleteCompany(companyId, true);

            // Assert
            _mockRepositoryManager.Verify(x => x.Company.GetCompany(companyId, true), Times.Once);
            _mockRepositoryManager.Verify(x => x.Company.DeleteCompany(company), Times.Once);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        [Fact]
        public async Task DeleteCompany_WithInvalidCompanyId_ThrowsCompanyNotFoundException()
        {
            // Arrange
            int companyId = 999;
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyId, true))
                .ReturnsAsync((Company)null);

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() =>
                _companyService.DeleteCompany(companyId, true));
            _mockRepositoryManager.Verify(x => x.Company.DeleteCompany(It.IsAny<Company>()), Times.Never);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Never);
        }

        [Fact]
        public async Task DeleteCompany_CallsSaveChangesAfterDeletion()
        {
            // Arrange
            int companyId = 1;
            var company = new Company { Id = companyId, Name = "Delete Test" };

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyId, true))
                .ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);

            // Act
            await _companyService.DeleteCompany(companyId, true);

            // Assert
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        #endregion

        #region UpdateCompany Tests

        [Fact]
        public async Task UpdateCompany_WithValidCompanyId_UpdatesSuccessfully()
        {
            // Arrange
            var companyDto = new CompanyDto(1, "Updated Corp", "New Address");
            var existingCompany = new Company { Id = 1, Name = "Old Corp", Address = "Old Address" };

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyDto.Id, true))
                .ReturnsAsync(existingCompany);
            _mockMapper.Setup(x => x.Map(companyDto, existingCompany))
                .Callback<CompanyDto, Company>((src, dest) =>
                {
                    dest.Name = src.Name;
                    dest.Address = src.FullAddress;
                });
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);

            // Act
            await _companyService.UpdateCompany(companyDto);

            // Assert
            _mockRepositoryManager.Verify(x => x.Company.GetCompany(companyDto.Id, true), Times.Once);
            _mockMapper.Verify(x => x.Map(companyDto, existingCompany), Times.Once);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
            Assert.Equal("Updated Corp", existingCompany.Name);
            Assert.Equal("New Address", existingCompany.Address);
        }

        [Fact]
        public async Task UpdateCompany_WithInvalidCompanyId_ThrowsCompanyNotFoundException()
        {
            // Arrange
            var companyDto = new CompanyDto(999, "Ghost Corp", "Ghost Address");
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyDto.Id, true))
                .ReturnsAsync((Company)null);

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() =>
                _companyService.UpdateCompany(companyDto));
            _mockRepositoryManager.Verify(x => x.Company.GetCompany(companyDto.Id, true), Times.Once);
            _mockMapper.Verify(x => x.Map(It.IsAny<CompanyDto>(), It.IsAny<Company>()), Times.Never);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateCompany_MapsCompanyDtoToEntity()
        {
            // Arrange
            var companyDto = new CompanyDto(1, "Mapper Test", "Mapper Address");
            var existingCompany = new Company { Id = 1, Name = "Old" };

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyDto.Id, true))
                .ReturnsAsync(existingCompany);
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);

            // Act
            await _companyService.UpdateCompany(companyDto);

            // Assert
            _mockMapper.Verify(x => x.Map(companyDto, existingCompany), Times.Once);
        }

        [Fact]
        public async Task UpdateCompany_CallsSaveChangesAfterUpdate()
        {
            // Arrange
            var companyDto = new CompanyDto(1, "Save Test", "Save Address");
            var existingCompany = new Company { Id = 1 };

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyDto.Id, true))
                .ReturnsAsync(existingCompany);
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);

            // Act
            await _companyService.UpdateCompany(companyDto);

            // Assert
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        #endregion
    }
}
