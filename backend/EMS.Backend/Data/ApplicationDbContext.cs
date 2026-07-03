using EMS.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.AttendanceRecords)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed departments
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Engineering", Description = "Software development team" },
                new Department { Id = 2, Name = "Human Resources", Description = "HR and recruitment" },
                new Department { Id = 3, Name = "Sales", Description = "Sales and business development" },
                new Department { Id = 4, Name = "Finance", Description = "Accounts and finance" }
            );
        }
    }
}
