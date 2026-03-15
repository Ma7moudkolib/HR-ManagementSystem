using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Enums;
using Entities.Models;
using Moq;
using Service;
using Service.Contracts;
using Shared.DataTransferObjects;
using Xunit;

namespace CompanyEmployees.Tests
{
    public class LeaveServiceTests
    {
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ILeaveService _leaveService;

        public LeaveServiceTests()
        {
            _mockRepositoryManager = new Mock<IRepositoryManager>();
            _mockMapper = new Mock<IMapper>();
            _leaveService = new LeaveService(
                _mockRepositoryManager.Object,
                _mockMapper.Object);
        }

        #region CreateLeaveRequestAsync Tests

        [Fact]
        public async Task CreateLeaveRequestAsync_WithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var leaveRequestDto = new CreateLeaveRequestDto
            {
                EmployeeId = 1,
                LeaveTypeId = 1,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(3)
            };
            var leaveType = new LeaveType { Id = 1, Name = "Paid Leave", IsPaid = false };
            var leaveRequest = new LeaveRequest { EmployeeId = 1, LeaveTypeId = 1, TotalDays = 2 };

            _mockRepositoryManager.Setup(x => x.leaveTypeRepository.GetLeaveTypeByIdAsync(1, false)).ReturnsAsync(leaveType);
            _mockRepositoryManager.Setup(x => x.LeaveBalance.GetBalanceAsync(1, 1, false)).ReturnsAsync((LeaveBalance)null);
            _mockMapper.Setup(x => x.Map<LeaveRequest>(leaveRequestDto)).Returns(leaveRequest);
            _mockRepositoryManager.Setup(x => x.LeaveRequest.CreateLeaveRequestAsync(It.IsAny<LeaveRequest>())).Verifiable();
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _leaveService.CreateLeaveRequestAsync(leaveRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Leave request created successfully", result.message);
        }

        [Fact]
        public async Task CreateLeaveRequestAsync_WithInvalidDateRange_ReturnsFailureResponse()
        {
            // Arrange
            var leaveRequestDto = new CreateLeaveRequestDto
            {
                EmployeeId = 1,
                LeaveTypeId = 1,
                StartDate = DateTime.UtcNow.AddDays(5),
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            // Act
            var result = await _leaveService.CreateLeaveRequestAsync(leaveRequestDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid leave date range", result.message);
        }

        [Fact]
        public async Task CreateLeaveRequestAsync_WithInsufficientBalance_ReturnsFailureResponse()
        {
            // Arrange
            var leaveRequestDto = new CreateLeaveRequestDto
            {
                EmployeeId = 1,
                LeaveTypeId = 1,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(6)
            };
            var leaveType = new LeaveType { Id = 1, Name = "Paid Leave", IsPaid = true };
            var leaveBalance = new LeaveBalance { EmployeeId = 1, LeaveTypeId = 1, RemainingDays = 2 };

            _mockRepositoryManager.Setup(x => x.leaveTypeRepository.GetLeaveTypeByIdAsync(1, false)).ReturnsAsync(leaveType);
            _mockRepositoryManager.Setup(x => x.LeaveBalance.GetBalanceAsync(1, 1, false)).ReturnsAsync(leaveBalance);

            // Act
            var result = await _leaveService.CreateLeaveRequestAsync(leaveRequestDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Insufficient leave balance", result.message);
        }

        #endregion

        #region ApproveLeaveAsync Tests

        [Fact]
        public async Task ApproveLeaveAsync_WithValidLeaveRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var leaveRequest = new LeaveRequest { Id = 1, EmployeeId = 1, LeaveTypeId = 1, Status = LeaveStatus.Pending, TotalDays = 2 };
            var leaveType = new LeaveType { Id = 1, Name = "Paid Leave", IsPaid = false };

            _mockRepositoryManager.Setup(x => x.LeaveRequest.GetLeaveRequestByIdAsync(1, true)).ReturnsAsync(leaveRequest);
            _mockRepositoryManager.Setup(x => x.leaveTypeRepository.GetLeaveTypeByIdAsync(1, false)).ReturnsAsync(leaveType);
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _leaveService.ApproveLeaveAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Leave approved successfully", result.message);
            Assert.Equal(LeaveStatus.Approved, leaveRequest.Status);
        }

        [Fact]
        public async Task ApproveLeaveAsync_WithNonExistentRequest_ReturnsFailureResponse()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.LeaveRequest.GetLeaveRequestByIdAsync(999, true)).ReturnsAsync((LeaveRequest)null);

            // Act
            var result = await _leaveService.ApproveLeaveAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Leave request not found", result.message);
        }

        [Fact]
        public async Task ApproveLeaveAsync_WithAlreadyProcessedRequest_ReturnsFailureResponse()
        {
            // Arrange
            var leaveRequest = new LeaveRequest { Id = 1, Status = LeaveStatus.Approved };
            _mockRepositoryManager.Setup(x => x.LeaveRequest.GetLeaveRequestByIdAsync(1, true)).ReturnsAsync(leaveRequest);

            // Act
            var result = await _leaveService.ApproveLeaveAsync(1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Leave already processed", result.message);
        }

        [Fact]
        public async Task ApproveLeaveAsync_WithPaidLeaveAndBalance_DeductsDays()
        {
            // Arrange
            var leaveRequest = new LeaveRequest { Id = 1, EmployeeId = 1, LeaveTypeId = 1, Status = LeaveStatus.Pending, TotalDays = 2 };
            var leaveType = new LeaveType { Id = 1, Name = "Paid Leave", IsPaid = true };
            var leaveBalance = new LeaveBalance { EmployeeId = 1, LeaveTypeId = 1, RemainingDays = 10 };

            _mockRepositoryManager.Setup(x => x.LeaveRequest.GetLeaveRequestByIdAsync(1, true)).ReturnsAsync(leaveRequest);
            _mockRepositoryManager.Setup(x => x.leaveTypeRepository.GetLeaveTypeByIdAsync(1, false)).ReturnsAsync(leaveType);
            _mockRepositoryManager.Setup(x => x.LeaveBalance.GetBalanceAsync(1, 1, true)).ReturnsAsync(leaveBalance);
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _leaveService.ApproveLeaveAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(8, leaveBalance.RemainingDays);
        }

        #endregion

        #region RejectLeaveAsync Tests

        [Fact]
        public async Task RejectLeaveAsync_WithValidLeaveRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var leaveRequest = new LeaveRequest { Id = 1, Status = LeaveStatus.Pending };
            _mockRepositoryManager.Setup(x => x.LeaveRequest.GetLeaveRequestByIdAsync(1, true)).ReturnsAsync(leaveRequest);
            _mockRepositoryManager.Setup(x => x.savechanges()).Returns(Task.CompletedTask);

            // Act
            var result = await _leaveService.RejectLeaveAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Leave rejected", result.message);
            Assert.Equal(LeaveStatus.Rejected, leaveRequest.Status);
        }

        [Fact]
        public async Task RejectLeaveAsync_WithNonExistentRequest_ReturnsFailureResponse()
        {
            // Arrange
            _mockRepositoryManager.Setup(x => x.LeaveRequest.GetLeaveRequestByIdAsync(999, true)).ReturnsAsync((LeaveRequest)null);

            // Act
            var result = await _leaveService.RejectLeaveAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Leave request not found", result.message);
        }

        [Fact]
        public async Task RejectLeaveAsync_WithAlreadyProcessedRequest_ReturnsFailureResponse()
        {
            // Arrange
            var leaveRequest = new LeaveRequest { Id = 1, Status = LeaveStatus.Approved };
            _mockRepositoryManager.Setup(x => x.LeaveRequest.GetLeaveRequestByIdAsync(1, true)).ReturnsAsync(leaveRequest);

            // Act
            var result = await _leaveService.RejectLeaveAsync(1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Leave already processed", result.message);
        }

        #endregion

        #region GetEmployeeLeavesAsync Tests

        [Fact]
        public async Task GetEmployeeLeavesAsync_WithValidEmployeeId_ReturnsLeaveList()
        {
            // Arrange
            var leaveRequests = new List<LeaveRequest>
            {
                new LeaveRequest { Id = 1, EmployeeId = 1, Status = LeaveStatus.Pending },
                new LeaveRequest { Id = 2, EmployeeId = 1, Status = LeaveStatus.Approved }
            };
            var leaveDtos = new List<LeaveRequestDto>
            {
                new LeaveRequestDto { Id = 1 },
                new LeaveRequestDto { Id = 2 }
            };

            _mockRepositoryManager.Setup(x => x.LeaveRequest.GetByEmployeeAsync(1, false)).ReturnsAsync(leaveRequests);
            _mockMapper.Setup(x => x.Map<IEnumerable<LeaveRequestDto>>(leaveRequests)).Returns(leaveDtos);

            // Act
            var result = await _leaveService.GetEmployeeLeavesAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<LeaveRequestDto>)result).Count);
        }

        #endregion

        #region GetEmployeeLeaveBalancesAsync Tests

        [Fact]
        public async Task GetEmployeeLeaveBalancesAsync_WithValidEmployeeId_ReturnsBalanceList()
        {
            // Arrange
            var balances = new List<LeaveBalance>
            {
                new LeaveBalance { EmployeeId = 1, LeaveTypeId = 1, RemainingDays = 10 },
                new LeaveBalance { EmployeeId = 1, LeaveTypeId = 2, RemainingDays = 5 }
            };
            var balanceDtos = new List<LeaveBalanceDto>
            {
                new LeaveBalanceDto { LeaveType = "Annual", RemainingDays = 10 },
                new LeaveBalanceDto { LeaveType = "Sick", RemainingDays = 5 }
            };

            _mockRepositoryManager.Setup(x => x.LeaveBalance.GetBalancesForEmployeeAsync(1, false)).ReturnsAsync(balances);
            _mockMapper.Setup(x => x.Map<IEnumerable<LeaveBalanceDto>>(balances)).Returns(balanceDtos);

            // Act
            var result = await _leaveService.GetEmployeeLeaveBalancesAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<LeaveBalanceDto>)result).Count);
        }

        #endregion
    }
}
