namespace Shared.DataTransferObjects
{
    public class PayrollRecordDto
    {
        public int Id { get; set; }
        public int PayrollPeriodId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }

        // Salary components
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }

        // Attendance details
        public int WorkingDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int PaidLeaveDays { get; set; }
        public int UnpaidLeaveDays { get; set; }

        // Calculations
        public decimal DailyRate { get; set; }
        public decimal AbsentDeduction { get; set; }
        public decimal UnpaidLeaveDeduction { get; set; }
        public decimal NetSalary { get; set; }

        public string Remarks { get; set; }
        public DateTime CalculatedAt { get; set; }
    }

}
