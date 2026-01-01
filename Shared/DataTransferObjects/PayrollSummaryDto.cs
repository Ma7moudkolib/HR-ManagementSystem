namespace Shared.DataTransferObjects
{
    public class PayrollSummaryDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public string PeriodDisplay { get; set; }
        public bool IsClosed { get; set; }
        public int TotalEmployees { get; set; }
        public decimal TotalNetSalary { get; set; }
    }

}
