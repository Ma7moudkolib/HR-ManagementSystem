namespace Entities.Models
{
    public class LeaveType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPaid { get; set; } 
        public int DefaultDaysPerYear { get; set; }

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
    }


}
