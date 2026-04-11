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
    public class PayrollServiceTests
    {
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IPayrollService _payrollService;

        public PayrollServiceTests()
        {
            _mockRepositoryManager = new Mock<IRepositoryManager>();
            _mockMapper = new Mock<IMapper>();
            _payrollService = new PayrollService(
                _mockRepositoryManager.Object,
                _mockMapper.Object);
        }

        #region GeneratePayrollAsync Tests

        [Fact]
        public async Task GeneratePayrollAsync_WithValidData_ReturnsPayrollPeriodDto()
        {
            // Arrange
            var payrollRequest = new GeneratePayrollRequestDto { CompanyId = 1, Year = 2024, Month = 1, GeneratedBy = "Admin" };
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Salary = 50000, CompanyId = 1 },
                new Employee { Id = 2, Name = "Jane Smith", Salary = 60000, CompanyId = 1 }
            };
            var payrollPeriod = new PayrollPeriod { Id = 1, CompanyId = 1, Year = 2024, Month = 1, GeneratedBy = "Admin", TotalEmployees = 2 };
            var payrollDto = new PayrollPeriodDto { Id = 1, CompanyId = 1, Year = 2024, Month = 1 };

            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByCompanyAndMonthAsync(1, 2024, 1)).ReturnsAsync((PayrollPeriod)null);
            _mockRepositoryManager.Setup(x => x.Employee.GetEmployees(1, false)).ReturnsAsync(employees);
            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdForMonthAsync(It.IsAny<int>(), 2024, 1, false)).ReturnsAsync(new List<Attendance>());
            _mockRepositoryManager.Setup(x => x.LeaveRequest.GetByEmployeeAsync(It.IsAny<int>(), false)).ReturnsAsync(new List<LeaveRequest>());
            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.AddAsync(It.IsAny<PayrollPeriod>())).Verifiable();
            _mockRepositoryManager.Setup(x => x.payrollRecordRepository.AddRangeAsync(It.IsAny<IEnumerable<PayrollRecord>>())).Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<PayrollPeriodDto>(It.IsAny<PayrollPeriod>())).Returns(payrollDto);

            // Act
            var result = await _payrollService.GeneratePayrollAsync(payrollRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CompanyId);
            Assert.Equal(2024, result.Year);
        }

        [Fact]
        public async Task GeneratePayrollAsync_WithInvalidMonth_ThrowsArgumentException()
        {
            // Arrange
            var payrollRequest = new GeneratePayrollRequestDto { CompanyId = 1, Year = 2024, Month = 13, GeneratedBy = "Admin" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _payrollService.GeneratePayrollAsync(payrollRequest));
        }

        [Fact]
        public async Task GeneratePayrollAsync_WithExistingClosedPayroll_ThrowsInvalidOperationException()
        {
            // Arrange
            var payrollRequest = new GeneratePayrollRequestDto { CompanyId = 1, Year = 2024, Month = 1, GeneratedBy = "Admin" };
            var existingPayroll = new PayrollPeriod { Id = 1, IsClosed = true };

            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByCompanyAndMonthAsync(1, 2024, 1)).ReturnsAsync(existingPayroll);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _payrollService.GeneratePayrollAsync(payrollRequest));
            Assert.Contains("already closed", exception.Message);
        }

        [Fact]
        public async Task GeneratePayrollAsync_WithNoEmployees_ThrowsInvalidOperationException()
        {
            // Arrange
            var payrollRequest = new GeneratePayrollRequestDto { CompanyId = 1, Year = 2024, Month = 1, GeneratedBy = "Admin" };

            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByCompanyAndMonthAsync(1, 2024, 1)).ReturnsAsync((PayrollPeriod)null);
            _mockRepositoryManager.Setup(x => x.Employee.GetEmployees(1, false)).ReturnsAsync(new List<Employee>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _payrollService.GeneratePayrollAsync(payrollRequest));
            Assert.Contains("No active employees found", exception.Message);
        }

        #endregion

        #region GetPayrollByMonthAsync Tests

        [Fact]
        public async Task GetPayrollByMonthAsync_WithValidData_ReturnsPayrollPeriodDto()
        {
            // Arrange
            var payrollPeriod = new PayrollPeriod { Id = 1, CompanyId = 1, Year = 2024, Month = 1 };
            var payrollDto = new PayrollPeriodDto { Id = 1, CompanyId = 1, Year = 2024, Month = 1 };

            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByCompanyAndMonthAsync(1, 2024, 1)).ReturnsAsync(payrollPeriod);
            _mockMapper.Setup(x => x.Map<PayrollPeriodDto>(payrollPeriod)).Returns(payrollDto);

            // Act
            var result = await _payrollService.GetPayrollByMonthAsync(1, 2024, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CompanyId);
            Assert.Equal(2024, result.Year);
        }

        [Fact]
        public async Task GetPayrollByMonthAsync_WithNonExistentPayroll_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByCompanyAndMonthAsync(1, 2024, 1)).ReturnsAsync((PayrollPeriod)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _payrollService.GetPayrollByMonthAsync(1, 2024, 1));
            Assert.Contains("Payroll not found", exception.Message);
        }

        #endregion

        #region GetEmployeePayrollAsync Tests

        [Fact]
        public async Task GetEmployeePayrollAsync_WithValidData_ReturnsPayrollRecordDto()
        {
            // Arrange
            var employee = new Employee { Id = 1, CompanyId = 1, Name = "John Doe" };
            var payrollPeriod = new PayrollPeriod { Id = 1, CompanyId = 1, Year = 2024, Month = 1 };
            var payrollRecord = new PayrollRecord { Id = 1, EmployeeId = 1, PayrollPeriodId = 1 };
            var payrollRecordDto = new PayrollRecordDto { EmployeeId = 1 };

            _mockRepositoryManager.Setup(x => x.Employee.GetEmployee(1, 1, false)).ReturnsAsync(employee);
            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByCompanyAndMonthAsync(1, 2024, 1)).ReturnsAsync(payrollPeriod);
            _mockRepositoryManager.Setup(x => x.payrollRecordRepository.GetByEmployeeAndPeriodAsync(1, 1)).ReturnsAsync(payrollRecord);
            _mockMapper.Setup(x => x.Map<PayrollRecordDto>(payrollRecord)).Returns(payrollRecordDto);

            // Act
            var result = await _payrollService.GetEmployeePayrollAsync(1, 1, 2024, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.EmployeeId);
        }

        [Fact]
        public async Task GetEmployeePayrollAsync_WithNonExistentEmployee_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.Employee.GetEmployee(1, 999, false)).ReturnsAsync((Employee)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _payrollService.GetEmployeePayrollAsync(1, 999, 2024, 1));
            Assert.Contains("Employee 999 not found", exception.Message);
        }

        [Fact]
        public async Task GetEmployeePayrollAsync_WithNonExistentPayroll_ThrowsKeyNotFoundException()
        {
            // Arrange
            var employee = new Employee { Id = 1, CompanyId = 1 };

            _mockRepositoryManager.Setup(x => x.Employee.GetEmployee(1, 1, false)).ReturnsAsync(employee);
            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByCompanyAndMonthAsync(1, 2024, 1)).ReturnsAsync((PayrollPeriod)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _payrollService.GetEmployeePayrollAsync(1, 1, 2024, 1));
            Assert.Contains("Payroll not found", exception.Message);
        }

        [Fact]
        public async Task GetEmployeePayrollAsync_WithNonExistentPayrollRecord_ThrowsKeyNotFoundException()
        {
            // Arrange
            var employee = new Employee { Id = 1, CompanyId = 1 };
            var payrollPeriod = new PayrollPeriod { Id = 1, CompanyId = 1 };

            _mockRepositoryManager.Setup(x => x.Employee.GetEmployee(1, 1, false)).ReturnsAsync(employee);
            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByCompanyAndMonthAsync(1, 2024, 1)).ReturnsAsync(payrollPeriod);
            _mockRepositoryManager.Setup(x => x.payrollRecordRepository.GetByEmployeeAndPeriodAsync(1, 1)).ReturnsAsync((PayrollRecord)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _payrollService.GetEmployeePayrollAsync(1, 1, 2024, 1));
            Assert.Contains("Payroll record not found", exception.Message);
        }

        #endregion

        #region ClosePayrollAsync Tests

        [Fact]
        public async Task ClosePayrollAsync_WithValidPayrollPeriod_ReturnsClosedPayrollDto()
        {
            // Arrange
            var payrollPeriod = new PayrollPeriod { Id = 1, IsClosed = false };
            var payrollDto = new PayrollPeriodDto { Id = 1, IsClosed = true };

            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByIdAsync(1)).ReturnsAsync(payrollPeriod);
            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.UpdateAsync(payrollPeriod)).Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<PayrollPeriodDto>(payrollPeriod)).Returns(payrollDto);

            // Act
            var result = await _payrollService.ClosePayrollAsync(1, "Admin");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsClosed);
            Assert.True(payrollPeriod.IsClosed);
            Assert.Equal("Admin", payrollPeriod.ClosedBy);
        }

        [Fact]
        public async Task ClosePayrollAsync_WithNonExistentPayroll_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByIdAsync(999)).ReturnsAsync((PayrollPeriod)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _payrollService.ClosePayrollAsync(999, "Admin"));
            Assert.Contains("Payroll period 999 not found", exception.Message);
        }

        [Fact]
        public async Task ClosePayrollAsync_WithAlreadyClosedPayroll_ThrowsInvalidOperationException()
        {
            // Arrange
            var payrollPeriod = new PayrollPeriod { Id = 1, IsClosed = true };
            _mockRepositoryManager.Setup(x => x.payrollPeriodRepository.GetByIdAsync(1)).ReturnsAsync(payrollPeriod);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _payrollService.ClosePayrollAsync(1, "Admin"));
            Assert.Contains("already closed", exception.Message);
        }

        #endregion
    }
}
