using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;
namespace Repository.DatabaseContext
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PayrollPeriod>(entity =>
            {
                entity.ToTable("PayrollPeriods");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.GeneratedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ClosedBy)
                    .HasMaxLength(100);

                entity.Property(e => e.TotalBasicSalary)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TotalAllowances)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TotalDeductions)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TotalNetSalary)
                    .HasColumnType("decimal(18,2)");

                // Unique constraint: one payroll per company per month
                entity.HasIndex(e => new { e.CompanyId, e.Year, e.Month })
                    .IsUnique()
                    .HasDatabaseName("IX_PayrollPeriod_CompanyYearMonth");

                // Foreign key to Company
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One-to-many relationship with PayrollRecords
                entity.HasMany(e => e.PayrollRecords)
                    .WithOne(r => r.PayrollPeriod)
                    .HasForeignKey(r => r.PayrollPeriodId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<PayrollRecord>(entity =>
            {
                entity.ToTable("PayrollRecords");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EmployeeName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.EmployeeCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BasicSalary)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Allowances)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Deductions)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DailyRate)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.AbsentDeduction)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.UnpaidLeaveDeduction)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.NetSalary)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(500);

                // Unique constraint: one record per employee per payroll period
                entity.HasIndex(e => new { e.PayrollPeriodId, e.EmployeeId })
                    .IsUnique()
                    .HasDatabaseName("IX_PayrollRecord_PeriodEmployee");

                // Index for employee queries
                entity.HasIndex(e => e.EmployeeId)
                    .HasDatabaseName("IX_PayrollRecord_Employee");

                // Foreign key to Employee
                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Foreign key to PayrollPeriod is handled in PayrollPeriod configuration
            });
            // Company -> Departments
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Departments)
                .WithOne(d => d.Company)
                .HasForeignKey(d => d.CompanyId);

            // Company -> Employees
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Employees)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.CompanyId);

            // Department -> Employees
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId);
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveRequestConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveBalanceConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveTypeConfiguration());
        }
        public DbSet<Company>? Companies { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Department>? Departments { get; set; }
        public DbSet<Attendance> attendances { get; set; }
        public DbSet<LeaveRequest> leaveRequests { get; set; }
        public DbSet<LeaveBalance> leaveBalances { get; set; }
        public DbSet<LeaveType> leaveTypes { get; set; }
        public DbSet<PayrollPeriod> PayrollPeriods { get; set; }
        public DbSet<PayrollRecord> PayrollRecords { get; set; }
    }
}
