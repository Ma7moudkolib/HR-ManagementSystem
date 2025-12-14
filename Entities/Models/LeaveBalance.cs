namespace Entities.Models
{
    public class LeaveBalance
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public Guid LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }

        public int RemainingDays { get; set; }
    }


}
