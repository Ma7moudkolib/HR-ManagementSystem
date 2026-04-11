using Entities.Enums;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Repository.Configuration
{
    public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            // Example seed data
            builder.HasData(
                new LeaveRequest
                {
                    Id = 1,
                    EmployeeId = 1,
                    LeaveTypeId = 1,
                    StartDate = new DateTime(2024, 7, 1),
                    EndDate = new DateTime(2024, 7, 5),
                    TotalDays = 5,
                    Reason = "Vacation",
                    Status = LeaveStatus.Approved,
                    RequestDate = new DateTime(2024, 6, 15),
                    ActionDate = new DateTime(2024, 6, 16)
                }
            );
        }
    }
}
