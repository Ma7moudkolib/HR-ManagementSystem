using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Models;
using Moq;
using Service;
using Service.Contracts;
using Shared.DataTransferObjects;
using Xunit;

namespace CompanyEmployees.Tests
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly Mock<ILoggerManager> _mockLoggerManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IDepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _mockRepositoryManager = new Mock<IRepositoryManager>();
            _mockLoggerManager = new Mock<ILoggerManager>();
            _mockMapper = new Mock<IMapper>();
            _departmentService = new DepartmentService(
                _mockRepositoryManager.Object,
                _mockLoggerManager.Object,
                _mockMapper.Object);
        }

        #region GetDepartmentsAsync Tests

        [Fact]
        public async Task GetDepartmentsAsync_WithValidCompanyId_ReturnsDepartmentList()
        {
            // Arrange
            var company = new Company { Id = 1, Name = "Tech Corp" };
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "IT", CompanyId = 1 },
                new Department { Id = 2, Name = "HR", CompanyId = 1 }
            };
            var departmentDtos = new List<DepartmentDto>
            {
                new DepartmentDto(1, "IT", "Information Technology"),
                new DepartmentDto(2, "HR", "Human Resources")
            };

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Department.GetAllDepartments(1, false)).ReturnsAsync(departments);
            _mockMapper.Setup(x => x.Map<IEnumerable<DepartmentDto>>(departments)).Returns(departmentDtos);

            // Act
            var result = await _departmentService.GetDepartmentsAsync(1, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<DepartmentDto>)result).Count);
        }

        [Fact]
        public async Task GetDepartmentsAsync_WithInvalidCompanyId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(999, false)).ReturnsAsync((Company)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _departmentService.GetDepartmentsAsync(999, false));
            Assert.Contains("Company with id: 999 doesn't exist", exception.Message);
        }

        #endregion

        #region GetDepartmentAsync Tests

        [Fact]
        public async Task GetDepartmentAsync_WithValidIds_ReturnsDepartmentDto()
        {
            // Arrange
            var company = new Company { Id = 1 };
            var department = new Department { Id = 1, Name = "IT", CompanyId = 1 };
            var departmentDto = new DepartmentDto(1, "IT", "Information Technology");

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Department.GetDepartmentById(1, 1, false)).ReturnsAsync(department);
            _mockMapper.Setup(x => x.Map<DepartmentDto>(department)).Returns(departmentDto);

            // Act
            var result = await _departmentService.GetDepartmentAsync(1, 1, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("IT", result.Name);
        }

        [Fact]
        public async Task GetDepartmentAsync_WithInvalidCompanyId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(999, false)).ReturnsAsync((Company)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _departmentService.GetDepartmentAsync(999, 1, false));
            Assert.Contains("Company with id: 999 doesn't exist", exception.Message);
        }

        [Fact]
        public async Task GetDepartmentAsync_WithInvalidDepartmentId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var company = new Company { Id = 1 };
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Department.GetDepartmentById(1, 999, false)).ReturnsAsync((Department)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _departmentService.GetDepartmentAsync(1, 999, false));
            Assert.Contains("Department with id: 999 doesn't exist", exception.Message);
        }

        #endregion

        #region CreateDepartmentForCompanyAsync Tests

        [Fact]
        public async Task CreateDepartmentForCompanyAsync_WithValidCompanyId_ReturnsDepartmentDto()
        {
            // Arrange
            var company = new Company { Id = 1 };
            var departmentForCreation = new DepartmentCreateDto("IT", "Information Technology");
            var department = new Department { Id = 1, Name = "IT", CompanyId = 1 };
            var departmentDto = new DepartmentDto(1, "IT", "Information Technology");

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockMapper.Setup(x => x.Map<Department>(departmentForCreation)).Returns(department);
            _mockRepositoryManager.Setup(x => x.Department.CreateDepartment(1, department)).Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<DepartmentDto>(department)).Returns(departmentDto);

            // Act
            var result = await _departmentService.CreateDepartmentForCompanyAsync(1, departmentForCreation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("IT", result.Name);
        }

        [Fact]
        public async Task CreateDepartmentForCompanyAsync_WithInvalidCompanyId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var departmentForCreation = new DepartmentCreateDto("IT", "Information Technology");
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(999, false)).ReturnsAsync((Company)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _departmentService.CreateDepartmentForCompanyAsync(999, departmentForCreation));
            Assert.Contains("Company with id: 999 doesn't exist", exception.Message);
        }

        #endregion

        #region DeleteDepartmentForCompanyAsync Tests

        [Fact]
        public async Task DeleteDepartmentForCompanyAsync_WithValidIds_DeletesDepartment()
        {
            // Arrange
            var company = new Company { Id = 1 };
            var department = new Department { Id = 1, CompanyId = 1 };

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Department.GetDepartmentById(1, 1, false)).ReturnsAsync(department);
            _mockRepositoryManager.Setup(x => x.Department.DeleteDepartment(department)).Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);

            // Act
            await _departmentService.DeleteDepartmentForCompanyAsync(1, 1, false);

            // Assert
            _mockRepositoryManager.Verify(x => x.Department.DeleteDepartment(department), Times.Once);
        }

        [Fact]
        public async Task DeleteDepartmentForCompanyAsync_WithInvalidCompanyId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(999, false)).ReturnsAsync((Company)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _departmentService.DeleteDepartmentForCompanyAsync(999, 1, false));
            Assert.Contains("Company with id: 999 doesn't exist", exception.Message);
        }

        [Fact]
        public async Task DeleteDepartmentForCompanyAsync_WithInvalidDepartmentId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var company = new Company { Id = 1 };
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Department.GetDepartmentById(1, 999, false)).ReturnsAsync((Department)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _departmentService.DeleteDepartmentForCompanyAsync(1, 999, false));
            Assert.Contains("Department with id: 999 doesn't exist", exception.Message);
        }

        #endregion

        #region UpdateDepartmentForCompanyAsync Tests

        [Fact]
        public async Task UpdateDepartmentForCompanyAsync_WithValidIds_UpdatesDepartment()
        {
            // Arrange
            var company = new Company { Id = 1 };
            var department = new Department { Id = 1, Name = "IT", CompanyId = 1 };
            var departmentForUpdate = new DepartmentUpdateDto("Information Technology", "Renamed IT Department");

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, true)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Department.GetDepartmentById(1, 1, true)).ReturnsAsync(department);
            _mockMapper.Setup(x => x.Map(departmentForUpdate, department)).Returns(department);
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);

            // Act
            await _departmentService.UpdateDepartmentForCompanyAsync(1, 1, departmentForUpdate, true);

            // Assert
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        [Fact]
        public async Task UpdateDepartmentForCompanyAsync_WithInvalidCompanyId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var departmentForUpdate = new DepartmentUpdateDto("IT", "Information Technology");
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(999, true)).ReturnsAsync((Company)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _departmentService.UpdateDepartmentForCompanyAsync(999, 1, departmentForUpdate, true));
            Assert.Contains("Company with id: 999 doesn't exist", exception.Message);
        }

        [Fact]
        public async Task UpdateDepartmentForCompanyAsync_WithInvalidDepartmentId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var company = new Company { Id = 1 };
            var departmentForUpdate = new DepartmentUpdateDto("IT", "Information Technology");
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, true)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Department.GetDepartmentById(1, 999, true)).ReturnsAsync((Department)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _departmentService.UpdateDepartmentForCompanyAsync(1, 999, departmentForUpdate, true));
            Assert.Contains("Department with id: 999 doesn't exist", exception.Message);
        }

        #endregion
    }
}
