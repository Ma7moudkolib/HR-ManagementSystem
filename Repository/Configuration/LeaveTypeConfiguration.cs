using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Repository.Configuration
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.HasData(
                new LeaveType
                {
                    Id = 1,
                    Name = "Annual Leave",
                    IsPaid = true,
                    DefaultDaysPerYear = 20
                },
                new LeaveType
                {
                    Id = 2,
                    Name = "Sick Leave",
                    IsPaid = true,
                    DefaultDaysPerYear = 10
                },
                new LeaveType
                {
                    Id = 3,
                    Name = "Unpaid Leave",
                    IsPaid = false,
                    DefaultDaysPerYear = 0
                }
            );
        }
    }
}
