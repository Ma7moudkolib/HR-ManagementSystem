namespace Shared.DataTransferObjects
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public decimal WorkedHours { get; set; }
    }
}
