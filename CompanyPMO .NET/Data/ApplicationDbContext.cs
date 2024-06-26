﻿using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;
using Task = CompanyPMO_.NET.Models.Task;

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
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public DbSet<EmployeeIssue> EmployeeIssues { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Tier> Tiers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Changelog> Changelog { get; set; }
        public DbSet<ResetPasswordRequest> ResetPasswordRequests { get; set; }
        public DbSet<Workload> Workload { get; set; }
        public DbSet<Timeline> Timelines { get; set; }
        public DbSet<CompanyPicture> CompanyPictures { get; set; }
        public DbSet<ProjectPicture> ProjectPictures { get; set; }
        public DbSet<TaskPicture> TaskPictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary key definition
            modelBuilder.Entity<Address>().HasKey(a => a.AddressId);
            modelBuilder.Entity<Company>().HasKey(c => c.CompanyId);
            modelBuilder.Entity<Employee>().HasKey(e => e.EmployeeId);
            modelBuilder.Entity<EmployeeTask>().HasKey(t => t.RelationId);
            modelBuilder.Entity<EmployeeProject>().HasKey(p => p.RelationId);
            modelBuilder.Entity<EmployeeIssue>().HasKey(i => i.RelationId);
            modelBuilder.Entity<Issue>().HasKey(i => i.IssueId);
            modelBuilder.Entity<Notification>().HasKey(n => n.NotificationId);
            modelBuilder.Entity<Project>().HasKey(p => p.ProjectId);
            modelBuilder.Entity<Task>().HasKey(t => t.TaskId);
            modelBuilder.Entity<Tier>().HasKey(t => t.TierId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Changelog>().HasKey(c => c.LogId);
            modelBuilder.Entity<ResetPasswordRequest>().HasKey(r => r.RequestId);
            modelBuilder.Entity<Workload>().HasKey(w => w.WorkloadId);
            modelBuilder.Entity<Timeline>().HasKey(t => t.TimelineId);
            modelBuilder.Entity<CompanyPicture>().HasKey(c => c.CompanyPictureId);
            modelBuilder.Entity<ProjectPicture>().HasKey(p => p.ProjectPictureId);
            modelBuilder.Entity<TaskPicture>().HasKey(t => t.TaskPictureId);

            // Relationships

            modelBuilder.Entity<ResetPasswordRequest>()
                .HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasOne(t => t.Tier)
                .WithMany()
                .HasForeignKey(t => t.TierId);

            modelBuilder.Entity<Employee>()
                .HasOne(c => c.Company)
                .WithMany()
                .HasForeignKey(c => c.CompanyId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Supervisor)
                .WithMany(e => e.Employees)
                .HasForeignKey(s => s.SupervisorId);

            // One to one Employee-Workload relationship
            modelBuilder.Entity<Employee>()
                .HasOne(w => w.Workload)
                .WithOne(e => e.Employee)
                .HasForeignKey<Workload>(w => w.WorkloadId);

            modelBuilder.Entity<Timeline>()
                .HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId);

            modelBuilder.Entity<Timeline>()
                .HasOne(x => x.Tier)
                .WithMany()
                .HasForeignKey(x => x.TierId);

            modelBuilder.Entity<Timeline>()
                .HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<Timeline>()
                .HasOne(x => x.Task)
                .WithMany()
                .HasForeignKey(x => x.TaskId);

            modelBuilder.Entity<Timeline>()
                .HasOne(x => x.Issue)
                .WithMany()
                .HasForeignKey(x => x.IssueId);

            // Junction table for many to many

            modelBuilder.Entity<Employee>()
                .HasMany(p => p.Projects)
                .WithMany(e => e.Employees)
                .UsingEntity<EmployeeProject>();

            modelBuilder.Entity<Employee>()
                .HasMany(t => t.Tasks)
                .WithMany(e => e.Employees)
                .UsingEntity<EmployeeTask>();

            modelBuilder.Entity<Employee>()
                .HasMany(i => i.Issues)
                .WithMany(e => e.Employees)
                .UsingEntity<EmployeeIssue>();

            modelBuilder.Entity<Company>()
                .HasMany(x => x.Pictures)
                .WithOne(p => p.Company)
                .HasForeignKey(x => x.CompanyId);

            modelBuilder.Entity<Project>()
                .HasMany(x => x.Pictures)
                .WithOne(p => p.Project)
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<Task>()
                .HasMany(x => x.Pictures)
                .WithOne(p => p.Task)
                .HasForeignKey(x => x.TaskId);

            modelBuilder.Entity<Task>()
                .HasMany(t => t.Issues)
                .WithOne(t => t.Task)
                .HasForeignKey(i => i.TaskId);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Employees)
                .WithOne(c => c.Company)
                .HasForeignKey(c => c.CompanyId);

            modelBuilder.Entity<Company>()
                .HasMany(p => p.Projects)
                .WithOne(c => c.Company)
                .HasForeignKey(p => p.CompanyId);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectCreator)
                .WithMany()
                .HasForeignKey(p => p.ProjectCreatorId);

            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.TaskCreator)
                .WithMany()
                .HasForeignKey(t => t.TaskCreatorId);

            modelBuilder.Entity<Models.Task>()
                .HasOne(p => p.Project)
                .WithMany(t => t.Tasks)
                .HasForeignKey(p => p.ProjectId);

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.IssueCreator)
                .WithMany()
                .HasForeignKey(i => i.IssueCreatorId);

            modelBuilder.Entity<Notification>()
                .HasOne(x => x.Sender)
                .WithMany()
                .HasForeignKey(x => x.SenderId);

            modelBuilder.Entity<Notification>()
                .HasOne(x => x.Receiver)
                .WithMany()
                .HasForeignKey(x => x.ReceiverId);
        }
    }
}
