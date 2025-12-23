using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class addDataDeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1bf15d66-1b01-4735-bc39-30954a8fb0e5", null, "Manager", "MANAGER" },
                    { "45726e29-68d3-4d73-a7a7-8c81036491d4", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[,]
                {
                    { 1, "583 Wall Dr. Gwynn Oak, MD 21207", "USA", "IT_Solutions Ltd" },
                    { 2, "312 Forest Avenue, BF 923", "USA", "Admin_Solutions Ltd" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Human Resources Department", "HR" },
                    { 2, 1, "Information Technology Department", "IT" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "DepartmentId", "Email", "HireDate", "Name", "Phone", "Position", "Salary" },
                values: new object[,]
                {
                    { 1, 26, 1, 2, "sam.raiden@company.com", new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sam Raiden", "555-0101", "Software developer", 80000m },
                    { 2, 30, 1, 2, "jana.mcleaf@company.com", new DateTime(2019, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jana McLeaf", "555-0102", "Software developer", 85000m },
                    { 3, 35, 2, 1, "kane.miller@company.com", new DateTime(2018, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kane Miller", "555-0103", "Administrator", 90000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1bf15d66-1b01-4735-bc39-30954a8fb0e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45726e29-68d3-4d73-a7a7-8c81036491d4");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1);
        }
    }
}
