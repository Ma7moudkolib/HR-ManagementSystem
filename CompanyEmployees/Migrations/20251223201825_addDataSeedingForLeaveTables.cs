using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class addDataSeedingForLeaveTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "leaveTypes",
                columns: new[] { "Id", "DefaultDaysPerYear", "IsPaid", "Name" },
                values: new object[,]
                {
                    { 1, 20, true, "Annual Leave" },
                    { 2, 10, true, "Sick Leave" },
                    { 3, 0, false, "Unpaid Leave" }
                });

            migrationBuilder.InsertData(
                table: "leaveBalances",
                columns: new[] { "Id", "EmployeeId", "LeaveTypeId", "RemainingDays" },
                values: new object[] { 1, 1, 1, 15 });

            migrationBuilder.InsertData(
                table: "leaveRequests",
                columns: new[] { "Id", "ActionDate", "EmployeeId", "EndDate", "LeaveTypeId", "Reason", "RequestDate", "StartDate", "Status", "TotalDays" },
                values: new object[] { 1, new DateTime(2024, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2024, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Vacation", new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "leaveBalances",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "leaveRequests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "leaveTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "leaveTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "leaveTypes",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
