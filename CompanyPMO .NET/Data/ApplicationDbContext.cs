using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // *
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTask> EmployeeTasks { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Tier> Tiers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary key definition
            modelBuilder.Entity<Address>().HasKey(a => a.AddressId);
            modelBuilder.Entity<Company>().HasKey(c => c.CompanyId);
            modelBuilder.Entity<Employee>().HasKey(e => e.EmployeeId);
            modelBuilder.Entity<EmployeeTask>().HasKey(t => t.RelationId);
            modelBuilder.Entity<EmployeeProject>().HasKey(p => p.RelationId);
            modelBuilder.Entity<Issue>().HasKey(i => i.IssueId);
            modelBuilder.Entity<Notification>().HasKey(n => n.NotificationId);
            modelBuilder.Entity<Project>().HasKey(p => p.ProjectId);
            modelBuilder.Entity<Models.Task>().HasKey(t => t.TaskId);
            modelBuilder.Entity<Tier>().HasKey(t => t.TierId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);

            // Relationships

            modelBuilder.Entity<Employee>()
                .HasOne(t => t.Tier)
                .WithMany()
                .HasForeignKey(t => t.TierId);

            modelBuilder.Entity<Employee>()
                .HasOne(c => c.Company)
                .WithMany()
                .HasForeignKey(c => c.CompanyId);

            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Employee)
                .WithMany(p => p.EmployeeProjects)
                .HasForeignKey(ep => ep.EmployeeId);

            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Project)
                .WithMany(p => p.EmployeeProjects)
                .HasForeignKey(ep => ep.ProjectId);
        }
    }
}
