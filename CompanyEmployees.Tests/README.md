# Attendance Service Unit Tests

This project contains comprehensive unit tests for the `AttendanceService` class using xUnit and Moq.

## Overview

The test suite covers three main methods of the `AttendanceService`:
- **Checkin**: Employee check-in operation
- **Checkout**: Employee check-out operation  
- **GetEmployeeAttendance**: Retrieve attendance records for an employee

## Test Coverage

### Checkin Tests (3 tests)
1. **Checkin_WithValidEmployeeId_ReturnsSuccessResponse**
   - Tests successful check-in when employee hasn't checked in yet
   - Verifies the service creates an attendance record and saves changes
   - Confirms success response is returned

2. **Checkin_WithEmployeeAlreadyCheckedIn_ReturnsFailureResponse**
   - Tests check-in failure when employee already has an active check-in
   - Verifies appropriate error message is returned
   - Ensures no duplicate attendance records are created

3. **Checkin_SetCheckInTimeToNow**
   - Verifies that check-in time is set to the current UTC time
   - Ensures timestamp accuracy

### Checkout Tests (4 tests)
1. **Checkout_WithValidEmployeeCheckedIn_ReturnsSuccessResponse**
   - Tests successful check-out when employee is checked in
   - Verifies checkout record is updated and saved
   - Confirms success response is returned

2. **Checkout_WithEmployeeNotCheckedIn_ReturnsFailureResponse**
   - Tests check-out failure when employee hasn't checked in
   - Verifies appropriate error message
   - Ensures no changes are saved

3. **Checkout_CalculatesWorkedHoursCorrectly**
   - Validates worked hours calculation based on check-in and check-out times
   - Tests with 3-hour work period

4. **Checkout_SetsCheckOutTime**
   - Verifies checkout time is set to current UTC time
   - Confirms timestamp accuracy

### GetEmployeeAttendance Tests (3 tests)
1. **GetEmployeeAttendance_WithValidEmployeeId_ReturnsAttendanceList**
   - Tests retrieval of attendance records for a specific employee
   - Verifies correct number of records returned
   - Confirms DTOs are returned instead of model objects

2. **GetEmployeeAttendance_WithNoAttendanceRecords_ThrowsException**
   - Tests exception handling when no records exist
   - Verifies appropriate exception message

3. **GetEmployeeAttendance_MapsAttendanceToDto**
   - Confirms AutoMapper is correctly invoked to map models to DTOs

## Running the Tests

### Run all tests
```bash
dotnet test CompanyEmployees.Tests
```

### Run specific test class
```bash
dotnet test CompanyEmployees.Tests --filter "AttendanceServiceTests"
```

### Run with detailed output
```bash
dotnet test CompanyEmployees.Tests -v detailed
```

### Run with code coverage
```bash
dotnet test CompanyEmployees.Tests /p:CollectCoverage=true
```

## Dependencies

- **xUnit**: Testing framework
- **Moq**: Mocking library for dependencies
- **AutoMapper**: For DTO mapping tests

## Test Statistics

- **Total Tests**: 10
- **Test Classes**: 1
- **Coverage Areas**: 3 methods
  - Service logic
  - Data persistence
  - Error handling
  - DTO mapping

## Mocking Strategy

The tests use Moq to mock dependencies:
- `IRepositoryManager`: Repository abstraction for data access
- `IMapper`: AutoMapper for DTO transformations

This allows tests to run in isolation without requiring a real database.

## Future Enhancements

Consider adding tests for:
- Boundary conditions (e.g., invalid employee IDs)
- Concurrency scenarios
- Database exception handling
- Performance scenarios with large datasets
