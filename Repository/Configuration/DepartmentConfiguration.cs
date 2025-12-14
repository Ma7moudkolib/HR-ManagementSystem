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
                    Id = Guid.Parse("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"),
                    Name = "HR",
                    Description = "Human Resources Department",
                    CompanyId = Guid.Parse("c9d4c053-49b6-410c-bc78-2d54a9991870")
                },
                new Department
                {
                    Id = Guid.Parse("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e"),
                    Name = "IT",
                    Description = "Information Technology Department",
                    CompanyId = Guid.Parse("c9d4c053-49b6-410c-bc78-2d54a9991870")
                }
            );
        }
    }
}
