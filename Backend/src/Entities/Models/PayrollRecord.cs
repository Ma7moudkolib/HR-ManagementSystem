namespace Entities.Models
{
    public class PayrollRecord
    {
        public int Id { get; set; }
        public int PayrollPeriodId { get; set; }
        public int EmployeeId { get; set; }

        // Snapshot values - immutable after closing
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }

        // Calculation details
        public int WorkingDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int PaidLeaveDays { get; set; }
        public int UnpaidLeaveDays { get; set; }
        public decimal DailyRate { get; set; }
        public decimal AbsentDeduction { get; set; }
        public decimal UnpaidLeaveDeduction { get; set; }
        public decimal NetSalary { get; set; }

        // Additional information
        public string Remarks { get; set; }
        public DateTime CalculatedAt { get; set; }

        // Navigation properties
        public virtual PayrollPeriod PayrollPeriod { get; set; }
        public virtual Employee Employee { get; set; }

        public PayrollRecord()
        {
            CalculatedAt = DateTime.UtcNow;
        }
    }
}
