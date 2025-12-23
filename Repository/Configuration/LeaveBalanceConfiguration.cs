using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Repository.Configuration
{
    public class LeaveBalanceConfiguration : IEntityTypeConfiguration<LeaveBalance>
    {
        public void Configure(EntityTypeBuilder<LeaveBalance> builder)
        {
            // Example seed data
            builder.HasData(
                new LeaveBalance
                {
                    Id = 1,
                    EmployeeId = 1,
                    LeaveTypeId = 1,
                    RemainingDays = 15
                }
            );
        }
    }
}
