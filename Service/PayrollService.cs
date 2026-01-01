using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class PayrollService : IPayrollService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public PayrollService(
            IRepositoryManager repositoryManager,
            IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        public async Task<PayrollPeriodDto> GeneratePayrollAsync(GeneratePayrollRequestDto payrollRequestDto )
        {
            // Validate input
            if (payrollRequestDto.Month < 1 || payrollRequestDto.Month > 12)
                throw new ArgumentException("Month must be between 1 and 12");

            // Check if payroll already exists
            var existingPayroll = await _repositoryManager.payrollPeriodRepository
                .GetByCompanyAndMonthAsync(payrollRequestDto.CompanyId, payrollRequestDto.Year, payrollRequestDto.Month);

            if (existingPayroll != null)
            {
                if (existingPayroll.IsClosed)
                    throw new InvalidOperationException(
                        $"Payroll for {payrollRequestDto.Year}-{payrollRequestDto.Month:D2} is already closed and cannot be regenerated");

                throw new InvalidOperationException(
                    $"Payroll for {payrollRequestDto.Year} - {payrollRequestDto.Month:D2} already exists. Close or delete the existing payroll first");
            }

            // Get all active employees for the company
            var employees = await _repositoryManager.Employee.GetEmployees(payrollRequestDto.CompanyId, false);
            var employeeList = employees.ToList();

            if (!employeeList.Any())
                throw new InvalidOperationException("No active employees found for this company");

            // Create payroll period
            var payrollPeriod = new PayrollPeriod
            {
                CompanyId = payrollRequestDto.CompanyId,
                Year = payrollRequestDto.Year,
                Month = payrollRequestDto.Month,
                GeneratedBy = payrollRequestDto.GeneratedBy!,
                GeneratedAt = DateTime.UtcNow,
                TotalEmployees = employeeList.Count
            };

            // Calculate working days for the month
            var workingDays = CalculateWorkingDays(payrollRequestDto.Year, payrollRequestDto.Month);

            // Generate payroll records for each employee
            var payrollRecords = new List<PayrollRecord>();
            decimal totalBasicSalary = 0;
            decimal totalAllowances = 0;
            decimal totalDeductions = 0;
            decimal totalNetSalary = 0;

            foreach (var employee in employeeList)
            {
                var record = await CalculateEmployeePayrollAsync(
                    employee, payrollRequestDto.Year, payrollRequestDto.Month, workingDays);

                record.PayrollPeriod = payrollPeriod;
                payrollRecords.Add(record);

                totalBasicSalary += record.BasicSalary;
                totalAllowances += record.Allowances;
                totalDeductions += record.Deductions;
                totalNetSalary += record.NetSalary;
            }

            // Update payroll period totals
            payrollPeriod.TotalBasicSalary = totalBasicSalary;
            payrollPeriod.TotalAllowances = totalAllowances;
            payrollPeriod.TotalDeductions = totalDeductions;
            payrollPeriod.TotalNetSalary = totalNetSalary;
            payrollPeriod.PayrollRecords = payrollRecords;

            // Save to database
            _repositoryManager.payrollPeriodRepository.AddAsync(payrollPeriod);
            _repositoryManager.payrollRecordRepository.AddRangeAsync(payrollRecords);
            await _repositoryManager.savechanges();
            var payrollPeriodDto = _mapper.Map<PayrollPeriodDto>(payrollPeriod);
            return payrollPeriodDto;
        }

        public async Task<PayrollPeriodDto> GetPayrollByMonthAsync(
            int companyId, int year, int month)
        {
            var payrollPeriod = await _repositoryManager.payrollPeriodRepository
                .GetByCompanyAndMonthAsync(companyId, year, month);

            if (payrollPeriod == null)
                throw new KeyNotFoundException(
                    $"Payroll not found for company {companyId} in {year}-{month:D2}");

            var payRollPeriodDto = _mapper.Map<PayrollPeriodDto>(payrollPeriod);
            return payRollPeriodDto;
        }

        public async Task<PayrollRecordDto> GetEmployeePayrollAsync(
           int companyId,int employeeId, int year, int month)
        {
            var employee = await _repositoryManager.Employee.GetEmployee(companyId,employeeId,false);
            if (employee == null)
                throw new KeyNotFoundException($"Employee {employeeId} not found");

            var payrollPeriod = await _repositoryManager.payrollPeriodRepository
                .GetByCompanyAndMonthAsync(employee.CompanyId, year, month);

            if (payrollPeriod == null)
                throw new KeyNotFoundException(
                    $"Payroll not found for {year}-{month:D2}");

            var payrollRecord = await _repositoryManager.payrollRecordRepository
                .GetByEmployeeAndPeriodAsync(employeeId, payrollPeriod.Id);

            if (payrollRecord == null)
                throw new KeyNotFoundException(
                    $"Payroll record not found for employee {employeeId} in {year}-{month:D2}");
            var payrollRecordDto = _mapper.Map<PayrollRecordDto>(payrollRecord);

            return payrollRecordDto;
        }

        public async Task<PayrollPeriodDto> ClosePayrollAsync(int payrollPeriodId, string closedBy)
        {
            var payrollPeriod = await _repositoryManager.payrollPeriodRepository.GetByIdAsync(payrollPeriodId);

            if (payrollPeriod == null)
                throw new KeyNotFoundException($"Payroll period {payrollPeriodId} not found");

            if (payrollPeriod.IsClosed)
                throw new InvalidOperationException("Payroll is already closed");

            payrollPeriod.IsClosed = true;
            payrollPeriod.ClosedAt = DateTime.UtcNow;
            payrollPeriod.ClosedBy = closedBy;

             _repositoryManager.payrollPeriodRepository.UpdateAsync(payrollPeriod);
            await _repositoryManager.savechanges();

            var payrollPeriodDto = _mapper.Map<PayrollPeriodDto>(payrollPeriod);
            return payrollPeriodDto;
        }

        private async Task<PayrollRecord> CalculateEmployeePayrollAsync(
           Employee employee, int year, int month, int workingDays)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Get attendance records (only with checkout)
            var attendanceRecords = await _repositoryManager.Attendance
                .GetAttendanceByEmployeeIdForMonthAsync(employee.Id, year, month,false);
            var presentDays = attendanceRecords
                .Select(a => a!.CheckIn.Date)
                .Distinct()
                .Count();

            // Get approved leave requests
            var leaveRequests = await _repositoryManager.LeaveRequest.GetByEmployeeAsync(
                employee.Id,false);
            // Calculate leave days in this month
            int paidLeaveDays = 0;
            int unpaidLeaveDays = 0;

            foreach (var leave in leaveRequests)
            {
                var leaveStart = leave.StartDate < startDate ? startDate : leave.StartDate;
                var leaveEnd = leave.EndDate > endDate ? endDate : leave.EndDate;
                var daysInMonth = (leaveEnd - leaveStart).Days + 1;

                if (leave.LeaveType.IsPaid)
                    paidLeaveDays += daysInMonth;
                else
                    unpaidLeaveDays += daysInMonth;
            }

            // Calculate absent days (working days - present days - leave days)
            var absentDays = Math.Max(0, workingDays - presentDays - paidLeaveDays - unpaidLeaveDays);

            // Calculate salary components
            var dailyRate = employee.Salary / workingDays;
            var unpaidLeaveDeduction = unpaidLeaveDays * dailyRate;
            var absentDeduction = absentDays * dailyRate;
            var netSalary = employee.Salary
                - unpaidLeaveDeduction
                - absentDeduction;

            // Create payroll record with snapshot values
            var payrollRecord = new PayrollRecord
            {
                EmployeeId = employee.Id,
                EmployeeName = $"{employee.Name}",
                DepartmentName = employee.Department?.Name ?? "N/A",
                BasicSalary = employee.Salary,
                WorkingDays = workingDays,
                PresentDays = presentDays,
                AbsentDays = absentDays,
                PaidLeaveDays = paidLeaveDays,
                UnpaidLeaveDays = unpaidLeaveDays,
                DailyRate = Math.Round(dailyRate, 2),
                AbsentDeduction = Math.Round(absentDeduction, 2),
                UnpaidLeaveDeduction = Math.Round(unpaidLeaveDeduction, 2),
                NetSalary = Math.Round(netSalary, 2),
                CalculatedAt = DateTime.UtcNow
            };

            // Add remarks if there are any deductions
            if (absentDays > 0 || unpaidLeaveDays > 0)
            {
                var remarks = new List<string>();
                if (absentDays > 0)
                    remarks.Add($"{absentDays} absent days");
                if (unpaidLeaveDays > 0)
                    remarks.Add($"{unpaidLeaveDays} unpaid leave days");
                payrollRecord.Remarks = string.Join(", ", remarks);
            }

            return payrollRecord;
        }

        private int CalculateWorkingDays(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var workingDays = 0;

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Exclude weekends (Saturday and Sunday)
                if (date.DayOfWeek != DayOfWeek.Saturday &&
                    date.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }

            return workingDays;
        }

        //private PayrollPeriodDto MapToPayrollPeriodDto(PayrollPeriod payrollPeriod)
        //{
        //    return new PayrollPeriodDto
        //    {
        //        Id = payrollPeriod.Id,
        //        CompanyId = payrollPeriod.CompanyId,
        //        CompanyName = payrollPeriod.Company?.Name,
        //        Year = payrollPeriod.Year,
        //        Month = payrollPeriod.Month,
        //        MonthName = new DateTime(payrollPeriod.Year, payrollPeriod.Month, 1)
        //            .ToString("MMMM"),
        //        GeneratedAt = payrollPeriod.GeneratedAt,
        //        GeneratedBy = payrollPeriod.GeneratedBy,
        //        IsClosed = payrollPeriod.IsClosed,
        //        ClosedAt = payrollPeriod.ClosedAt,
        //        ClosedBy = payrollPeriod.ClosedBy,
        //        TotalEmployees = payrollPeriod.TotalEmployees,
        //        TotalBasicSalary = payrollPeriod.TotalBasicSalary,
        //        TotalAllowances = payrollPeriod.TotalAllowances,
        //        TotalDeductions = payrollPeriod.TotalDeductions,
        //        TotalNetSalary = payrollPeriod.TotalNetSalary,
        //        PayrollRecords = payrollPeriod.PayrollRecords?
        //            .Select(MapToPayrollRecordDto)
        //            .ToList()
        //    };
        //}

        //private PayrollRecordDto MapToPayrollRecordDto(PayrollRecord record)
        //{
        //    return new PayrollRecordDto
        //    {
        //        Id = record.Id,
        //        PayrollPeriodId = record.PayrollPeriodId,
        //        EmployeeId = record.EmployeeId,
        //        EmployeeName = record.EmployeeName,
        //        EmployeeCode = record.EmployeeCode,
        //        DepartmentName = record.DepartmentName,
        //        BasicSalary = record.BasicSalary,
        //        Allowances = record.Allowances,
        //        Deductions = record.Deductions,
        //        WorkingDays = record.WorkingDays,
        //        PresentDays = record.PresentDays,
        //        AbsentDays = record.AbsentDays,
        //        PaidLeaveDays = record.PaidLeaveDays,
        //        UnpaidLeaveDays = record.UnpaidLeaveDays,
        //        DailyRate = record.DailyRate,
        //        AbsentDeduction = record.AbsentDeduction,
        //        UnpaidLeaveDeduction = record.UnpaidLeaveDeduction,
        //        NetSalary = record.NetSalary,
        //        Remarks = record.Remarks,
        //        CalculatedAt = record.CalculatedAt
        //    };
        //}
    }
}
