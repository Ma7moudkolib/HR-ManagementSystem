using System;
using System.Collections.Generic;
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
    public class EmployeeServiceTests
    {
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly Mock<ILoggerManager> _mockLoggerManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IEmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _mockRepositoryManager = new Mock<IRepositoryManager>();
            _mockLoggerManager = new Mock<ILoggerManager>();
            _mockMapper = new Mock<IMapper>();
            _employeeService = new EmployeeService(
                _mockRepositoryManager.Object,
                _mockLoggerManager.Object,
                _mockMapper.Object);
        }

        #region CreateEmployeeForCompany Tests

        [Fact]
        public async Task CreateEmployeeForCompany_WithValidCompanyId_ReturnsEmployeeDto()
        {
            // Arrange
            int companyId = 1;
            var company = new Company { Id = companyId, Name = "Tech Corp" };
            var employeeForCreation = new EmployeeForCreationDto("John Doe", 30, "Developer");
            var employee = new Employee { Name = "John Doe", Age = 30, Position = "Developer", CompanyId = companyId };
            var employeeDto = new EmployeeDto(1, "John Doe", 30, "Developer", "john@example.com", "123-456-7890", "50000");

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(companyId, false)).ReturnsAsync(company);
            _mockMapper.Setup(x => x.Map<Employee>(company)).Returns(employee);
            _mockRepositoryManager.Setup(x => x.Employee.CreateEmployeeForCompany(companyId, It.IsAny<Employee>())).Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<EmployeeDto>(employee)).Returns(employeeDto);

            // Act
            var result = await _employeeService.CreateEmployeeForCompany(companyId, employeeForCreation, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            _mockRepositoryManager.Verify(x => x.Company.GetCompany(companyId, false), Times.Once);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        [Fact]
        public async Task CreateEmployeeForCompany_WithInvalidCompanyId_ThrowsCompanyNotFoundException()
        {
            // Arrange
            var employeeForCreation = new EmployeeForCreationDto("John Doe", 30, "Developer");
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(999, false)).ReturnsAsync((Company)null);

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() =>
                _employeeService.CreateEmployeeForCompany(999, employeeForCreation, false));
        }

        #endregion

        #region GetEmployee Tests

        [Fact]
        public async Task GetEmployee_WithValidIds_ReturnsEmployeeDto()
        {
            // Arrange
            var company = new Company { Id = 1, Name = "Tech Corp" };
            var employee = new Employee { Id = 1, Name = "John Doe", Age = 30, Position = "Developer", CompanyId = 1 };
            var employeeDto = new EmployeeDto(1, "John Doe", 30, "Developer", "john@example.com", "123-456-7890", "50000");

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Employee.GetEmployee(1, 1, false)).ReturnsAsync(employee);
            _mockMapper.Setup(x => x.Map<EmployeeDto>(employee)).Returns(employeeDto);

            // Act
            var result = await _employeeService.GetEmployee(1, 1, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task GetEmployee_WithInvalidCompanyId_ThrowsCompanyNotFoundException()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(999, false)).ReturnsAsync((Company)null);

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() => _employeeService.GetEmployee(999, 1, false));
        }

        [Fact]
        public async Task GetEmployee_WithInvalidEmployeeId_ThrowsEmployeeNotFoundException()
        {
            // Arrange
            var company = new Company { Id = 1 };
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Employee.GetEmployee(1, 999, false)).ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeNotFoundException>(() => _employeeService.GetEmployee(1, 999, false));
        }

        #endregion

        #region GetEmployees Tests

        [Fact]
        public async Task GetEmployees_WithValidCompanyId_ReturnsEmployeeList()
        {
            // Arrange
            var company = new Company { Id = 1 };
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Age = 30, Position = "Developer" },
                new Employee { Id = 2, Name = "Jane Smith", Age = 28, Position = "Designer" }
            };
            var employeeDtos = new List<EmployeeDto>
            {
                new EmployeeDto(1, "John Doe", 30, "Developer", "john@example.com", "123-456-7890", "50000"),
                new EmployeeDto(2, "Jane Smith", 28, "Designer", "jane@example.com", "123-456-7891", "48000")
            };

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Employee.GetEmployees(1, false)).ReturnsAsync(employees);
            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeDto>>(employees)).Returns(employeeDtos);

            // Act
            var result = await _employeeService.GetEmployees(1, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<EmployeeDto>)result).Count);
        }

        [Fact]
        public async Task GetEmployees_WithInvalidCompanyId_ThrowsCompanyNotFoundException()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(999, false)).ReturnsAsync((Company)null);

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() => _employeeService.GetEmployees(999, false));
        }

        #endregion

        #region UpdateEmployeeForCompany Tests

        [Fact]
        public async Task UpdateEmployeeForCompany_WithValidIds_UpdatesEmployee()
        {
            // Arrange
            var company = new Company { Id = 1 };
            var employee = new Employee { Id = 1, Name = "John Doe", Age = 30, Position = "Developer", CompanyId = 1 };
            var employeeForUpdate = new EmployeeForUpdateDto("Johnny Doe", 31, "Senior Developer");

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, true)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Employee.GetEmployee(1, 1, true)).ReturnsAsync(employee);
            _mockMapper.Setup(x => x.Map(employeeForUpdate, employee)).Returns(employee);
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);

            // Act
            await _employeeService.UpdateEmployeeForCompany(1, 1, employeeForUpdate, true, true);

            // Assert
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeForCompany_WithInvalidCompanyId_ThrowsCompanyNotFoundException()
        {
            // Arrange
            var employeeForUpdate = new EmployeeForUpdateDto("Johnny Doe", 31, "Senior Developer");
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(999, true)).ReturnsAsync((Company)null);

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() =>
                _employeeService.UpdateEmployeeForCompany(999, 1, employeeForUpdate, true, true));
        }

        [Fact]
        public async Task UpdateEmployeeForCompany_WithInvalidEmployeeId_ThrowsEmployeeNotFoundException()
        {
            // Arrange
            var company = new Company { Id = 1 };
            var employeeForUpdate = new EmployeeForUpdateDto("Johnny Doe", 31, "Senior Developer");
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, true)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Employee.GetEmployee(1, 999, true)).ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeNotFoundException>(() =>
                _employeeService.UpdateEmployeeForCompany(1, 999, employeeForUpdate, true, true));
        }

        #endregion

        #region DeleteEmployeeForCompany Tests

        [Fact]
        public async Task DeleteEmployeeForCompany_WithValidIds_DeletesEmployee()
        {
            // Arrange
            var company = new Company { Id = 1 };
            var employee = new Employee { Id = 1, CompanyId = 1 };

            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Employee.GetEmployee(1, 1, false)).ReturnsAsync(employee);
            _mockRepositoryManager.Setup(x => x.Employee.DeleteEmployee(employee)).Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);

            // Act
            await _employeeService.DeleteEmployeeForCompany(1, 1, false);

            // Assert
            _mockRepositoryManager.Verify(x => x.Employee.DeleteEmployee(employee), Times.Once);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeForCompany_WithInvalidCompanyId_ThrowsCompanyNotFoundException()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(999, false)).ReturnsAsync((Company)null);

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() =>
                _employeeService.DeleteEmployeeForCompany(999, 1, false));
        }

        [Fact]
        public async Task DeleteEmployeeForCompany_WithInvalidEmployeeId_ThrowsEmployeeNotFoundException()
        {
            // Arrange
            var company = new Company { Id = 1 };
            _mockRepositoryManager.Setup(x => x.Company.GetCompany(1, false)).ReturnsAsync(company);
            _mockRepositoryManager.Setup(x => x.Employee.GetEmployee(1, 999, false)).ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeNotFoundException>(() =>
                _employeeService.DeleteEmployeeForCompany(1, 999, false));
        }

        #endregion
    }
}
