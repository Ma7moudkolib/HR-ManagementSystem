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
                    Id = 1,
                    Name = "Sam Raiden",
                    Age = 26,
                    Position = "Software developer",
                    Email = "sam.raiden@company.com",
                    Phone = "555-0101",
                    HireDate = new DateTime(2020, 1, 15),
                    Salary = 80000m,
                    CompanyId = 1,
                    DepartmentId = 2
                },
                new Employee
                {
                    Id = 2,
                    Name = "Jana McLeaf",
                    Age = 30,
                    Position = "Software developer",
                    Email = "jana.mcleaf@company.com",
                    Phone = "555-0102",
                    HireDate = new DateTime(2019, 5, 10),
                    Salary = 85000m,
                    CompanyId = 1,
                    DepartmentId = 2
                },
                new Employee
                {
                    Id = 3,
                    Name = "Kane Miller",
                    Age = 35,
                    Position = "Administrator",
                    Email = "kane.miller@company.com",
                    Phone = "555-0103",
                    HireDate = new DateTime(2018, 3, 20),
                    Salary = 90000m,
                    CompanyId = 2,
                    DepartmentId = 1
                });

        }
    }
}
