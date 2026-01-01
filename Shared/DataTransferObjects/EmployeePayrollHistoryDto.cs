namespace Shared.DataTransferObjects
{
    public class EmployeePayrollHistoryDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public List<PayrollRecordDto> PayrollHistory { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal AverageNetSalary { get; set; }
    }

}
