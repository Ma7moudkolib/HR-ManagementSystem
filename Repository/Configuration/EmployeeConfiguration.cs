using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData(
                new Employee
                {
                    Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                    Name = "Sam Raiden",
                    Age = 26,
                    Position = "Software developer",
                    Email = "sam.raiden@company.com",
                    Phone = "555-0101",
                    HireDate = new DateTime(2020, 1, 15),
                    Salary = 80000m,
                    CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    DepartmentId = new Guid("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e")
                },
                new Employee
                {
                    Id = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
                    Name = "Jana McLeaf",
                    Age = 30,
                    Position = "Software developer",
                    Email = "jana.mcleaf@company.com",
                    Phone = "555-0102",
                    HireDate = new DateTime(2019, 5, 10),
                    Salary = 85000m,
                    CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    DepartmentId = new Guid("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e")
                },
                new Employee
                {
                    Id = new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"),
                    Name = "Kane Miller",
                    Age = 35,
                    Position = "Administrator",
                    Email = "kane.miller@company.com",
                    Phone = "555-0103",
                    HireDate = new DateTime(2018, 3, 20),
                    Salary = 90000m,
                    CompanyId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                    DepartmentId = new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d")
                });

        }
    }
}
