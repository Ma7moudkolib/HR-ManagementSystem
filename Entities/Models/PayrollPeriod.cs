namespace Entities.Models
{
    public class PayrollPeriod
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string GeneratedBy { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string ClosedBy { get; set; }
        public int TotalEmployees { get; set; }
        public decimal TotalBasicSalary { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalNetSalary { get; set; }

        // Navigation properties
        public virtual Company Company { get; set; }
        public virtual ICollection<PayrollRecord> PayrollRecords { get; set; }

        public PayrollPeriod()
        {
            PayrollRecords = new HashSet<PayrollRecord>();
            GeneratedAt = DateTime.UtcNow;
            IsClosed = false;
        }
    }
}
