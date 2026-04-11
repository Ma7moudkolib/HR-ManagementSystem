using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Repository.Configuration
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(
                new Department
                {
                    Id = 1,
                    Name = "HR",
                    Description = "Human Resources Department",
                    CompanyId = 1
                },
                new Department
                {
                    Id = 2,
                    Name = "IT",
                    Description = "Information Technology Department",
                    CompanyId = 1
                }
            );
        }
    }
}
