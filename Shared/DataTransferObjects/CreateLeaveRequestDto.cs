namespace Shared.DataTransferObjects
{
    public class CreateLeaveRequestDto
    {
        public Guid EmployeeId { get; set; }
        public Guid LeaveTypeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Reason { get; set; }
    }

}
