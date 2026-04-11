namespace Shared.DataTransferObjects
{
    public class LeaveBalanceDto
    {
        public string LeaveType { get; set; }
        public int RemainingDays { get; set; }
        public bool IsPaid { get; set; }
    }

}
