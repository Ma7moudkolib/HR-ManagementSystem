namespace Entities.Models
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public decimal WorkedHours { get; set; }
        public Employee Employee { get; set; }

    }
}
