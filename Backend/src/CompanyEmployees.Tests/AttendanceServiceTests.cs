using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AttendanceServiceTests
    {
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AttendanceService _attendanceService;

        public AttendanceServiceTests()
        {
            _mockRepositoryManager = new Mock<IRepositoryManager>();
            _mockMapper = new Mock<IMapper>();
            _attendanceService = new AttendanceService(_mockRepositoryManager.Object, _mockMapper.Object);
        }

        #region Checkin Tests

        [Fact]
        public async Task Checkin_WithValidEmployeeId_ReturnsSuccessResponse()
        {
            // Arrange
            int employeeId = 1;
            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdAsync(employeeId, false))!
                .ReturnsAsync((IEnumerable<Attendance>)null);
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _attendanceService.Checkin(employeeId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Check-in successful", result.message);
            _mockRepositoryManager.Verify(x => x.Attendance.CreateAttendance(It.IsAny<Attendance>()), Times.Once);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        [Fact]
        public async Task Checkin_WithEmployeeAlreadyCheckedIn_ReturnsFailureResponse()
        {
            // Arrange
            int employeeId = 1;
            var existingAttendance = new List<Attendance>
            {
                new Attendance { EmployeeId = employeeId, CheckIn = DateTime.UtcNow }
            };
            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdAsync(employeeId, false))
                .ReturnsAsync(existingAttendance);

            // Act
            var result = await _attendanceService.Checkin(employeeId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Employee already checked in", result.message);
            _mockRepositoryManager.Verify(x => x.Attendance.CreateAttendance(It.IsAny<Attendance>()), Times.Never);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Never);
        }

        [Fact]
        public async Task Checkin_SetCheckInTimeToNow()
        {
            // Arrange
            int employeeId = 1;
            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdAsync(employeeId, false))
                .ReturnsAsync((IEnumerable<Attendance>)null);
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);

            var beforeTime = DateTime.UtcNow;

            // Act
            var result = await _attendanceService.Checkin(employeeId);

            var afterTime = DateTime.UtcNow;

            // Assert
            _mockRepositoryManager.Verify(
                x => x.Attendance.CreateAttendance(It.Is<Attendance>(a =>
                    a.EmployeeId == employeeId &&
                    a.CheckIn >= beforeTime &&
                    a.CheckIn <= afterTime)),
                Times.Once);
        }

        #endregion

        #region Checkout Tests

        [Fact]
        public async Task Checkout_WithValidEmployeeCheckedIn_ReturnsSuccessResponse()
        {
            // Arrange
            int employeeId = 1;
            var checkInTime = DateTime.UtcNow.AddHours(-2);
            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                CheckIn = checkInTime,
                CheckOut = null,
                WorkedHours = 0
            };

            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdForDayAsync(employeeId, It.IsAny<DateTime>(), true))
                .ReturnsAsync(attendance);
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _attendanceService.Checkout(employeeId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Check-out successful", result.message);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Once);
        }

        [Fact]
        public async Task Checkout_WithEmployeeNotCheckedIn_ReturnsFailureResponse()
        {
            // Arrange
            int employeeId = 1;
            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdForDayAsync(employeeId, It.IsAny<DateTime>(), true))
                .ReturnsAsync((Attendance)null);

            // Act
            var result = await _attendanceService.Checkout(employeeId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Employee has not checked in", result.message);
            _mockRepositoryManager.Verify(x => x.savechanges(), Times.Never);
        }

        [Fact]
        public async Task Checkout_CalculatesWorkedHoursCorrectly()
        {
            // Arrange
            int employeeId = 1;
            var checkInTime = DateTime.UtcNow.AddHours(-3);
            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                CheckIn = checkInTime,
                CheckOut = null,
                WorkedHours = 0
            };

            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdForDayAsync(employeeId, It.IsAny<DateTime>(), true))
                .ReturnsAsync(attendance);
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);

            // Act
            await _attendanceService.Checkout(employeeId);

            // Assert
            Assert.NotNull(attendance.CheckOut);
            Assert.True(attendance.WorkedHours >= 2.9m && attendance.WorkedHours <= 3.1m);
        }

        [Fact]
        public async Task Checkout_SetsCheckOutTime()
        {
            // Arrange
            int employeeId = 1;
            var checkInTime = DateTime.UtcNow.AddHours(-1);
            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                CheckIn = checkInTime,
                CheckOut = null,
                WorkedHours = 0
            };

            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdForDayAsync(employeeId, It.IsAny<DateTime>(), true))
                .ReturnsAsync(attendance);
            _mockRepositoryManager.Setup(x => x.savechanges())
                .Returns(Task.CompletedTask);

            var beforeTime = DateTime.UtcNow;

            // Act
            await _attendanceService.Checkout(employeeId);

            var afterTime = DateTime.UtcNow;

            // Assert
            Assert.NotNull(attendance.CheckOut);
            Assert.True(attendance.CheckOut >= beforeTime && attendance.CheckOut <= afterTime);
        }

        #endregion

        #region GetEmployeeAttendance Tests

        [Fact]
        public async Task GetEmployeeAttendance_WithValidEmployeeId_ReturnsAttendanceList()
        {
            // Arrange
            int employeeId = 1;
            var attendances = new List<Attendance>
            {
                new Attendance { Id = 1, EmployeeId = employeeId, CheckIn = DateTime.UtcNow.AddDays(-1), CheckOut = DateTime.UtcNow.AddDays(-1), WorkedHours = 8 },
                new Attendance { Id = 2, EmployeeId = employeeId, CheckIn = DateTime.UtcNow.AddDays(-2), CheckOut = DateTime.UtcNow.AddDays(-2), WorkedHours = 8 }
            };

            var attendanceDtos = new List<AttendanceDto>
            {
                new AttendanceDto { Id = 1, EmployeeId = employeeId, CheckIn = DateTime.UtcNow.AddDays(-1), CheckOut = DateTime.UtcNow.AddDays(-1), WorkedHours = 8 },
                new AttendanceDto { Id = 2, EmployeeId = employeeId, CheckIn = DateTime.UtcNow.AddDays(-2), CheckOut = DateTime.UtcNow.AddDays(-2), WorkedHours = 8 }
            };

            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdAsync(employeeId, false))
                .ReturnsAsync(attendances);
            _mockMapper.Setup(x => x.Map<IEnumerable<AttendanceDto>>(attendances))
                .Returns(attendanceDtos);

            // Act
            var result = await _attendanceService.GetEmployeeAttendance(employeeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.True(result.Any(a => a.Id == 1));
            Assert.True(result.Any(a => a.Id == 2));
            _mockMapper.Verify(x => x.Map<IEnumerable<AttendanceDto>>(attendances), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeAttendance_WithNoAttendanceRecords_ThrowsException()
        {
            // Arrange
            int employeeId = 1;
            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdAsync(employeeId, false))
                .ReturnsAsync((IEnumerable<Attendance>)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _attendanceService.GetEmployeeAttendance(employeeId));
            Assert.Equal("No attendance records found for the employee.", exception.Message);
        }

        [Fact]
        public async Task GetEmployeeAttendance_MapsAttendanceToDto()
        {
            // Arrange
            int employeeId = 1;
            var attendances = new List<Attendance>
            {
                new Attendance { Id = 1, EmployeeId = employeeId, CheckIn = DateTime.UtcNow, CheckOut = null, WorkedHours = 0 }
            };

            var attendanceDtos = new List<AttendanceDto>
            {
                new AttendanceDto { Id = 1, EmployeeId = employeeId, CheckIn = DateTime.UtcNow, CheckOut = null, WorkedHours = 0 }
            };

            _mockRepositoryManager.Setup(x => x.Attendance.GetAttendanceByEmployeeIdAsync(employeeId, false))
                .ReturnsAsync(attendances);
            _mockMapper.Setup(x => x.Map<IEnumerable<AttendanceDto>>(It.IsAny<IEnumerable<Attendance>>()))
                .Returns(attendanceDtos);

            // Act
            var result = await _attendanceService.GetEmployeeAttendance(employeeId);

            // Assert
            _mockMapper.Verify(x => x.Map<IEnumerable<AttendanceDto>>(attendances), Times.Once);
        }

        #endregion
    }
}
