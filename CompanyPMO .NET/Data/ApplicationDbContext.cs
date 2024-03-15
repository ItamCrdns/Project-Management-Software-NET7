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
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public DbSet<EmployeeIssue> EmployeeIssues { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Tier> Tiers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Changelog> Changelog { get; set; }
        public DbSet<ResetPasswordRequest> ResetPasswordRequests { get; set; }

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
            modelBuilder.Entity<Models.Task>().HasKey(t => t.TaskId);
            modelBuilder.Entity<Tier>().HasKey(t => t.TierId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Image>().HasKey(i => i.ImageId);
            modelBuilder.Entity<Changelog>().HasKey(c => c.LogId);
            modelBuilder.Entity<ResetPasswordRequest>().HasKey(r => r.RequestId);

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

            //modelBuilder.Entity<Project>()
            //    .HasOne(c => c.Company)
            //    .WithMany(p => p.Projects)
            //    .HasForeignKey(c => c.CompanyId);

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

            modelBuilder.Entity<Project>()
                .HasMany(i => i.Images)
                .WithOne(p => p.Project)
                .HasForeignKey(i => i.EntityId);

            modelBuilder.Entity<Models.Task>()
                .HasMany(i => i.Images)
                .WithOne(t => t.Task)
                .HasForeignKey(i => i.EntityId);

            modelBuilder.Entity<Models.Task>()
                .HasMany(t => t.Issues)
                .WithOne(t => t.Task)
                .HasForeignKey(i => i.TaskId);

            modelBuilder.Entity<Company>()
                .HasMany(i => i.Images)
                .WithOne(c => c.Company)
                .HasForeignKey(i => i.EntityId);

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
                .WithMany()
                .HasForeignKey(p => p.ProjectId);

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.IssueCreator)
                .WithMany()
                .HasForeignKey(i => i.IssueCreatorId);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "User1",
                    FirstName = "First",
                    LastName = "User",
                    Gender = "Male",
                    Created = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow
                },
                new User
                {
                    UserId = 2,
                    Username = "User2",
                    FirstName = "Second",
                    LastName = "User",
                    Gender = "Female",
                    Created = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow
                },
                new User
                {
                    UserId = 3,
                    Username = "User3",
                    FirstName = "Third",
                    LastName = "User",
                    Gender = "Male",
                    Created = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow
                });

            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    AddressId = 1,
                    StreetAddress = "123 Fake Street",
                    City = "Faketown",
                    State = "FS",
                    PostalCode = "12345",
                    Country = "Fake Country"
                },
                new Address
                {
                    AddressId = 2,
                    StreetAddress = "456 Fake Avenue",
                    City = "Fakecity",
                    State = "FC",
                    PostalCode = "67890",
                    Country = "Fake Country"
                },
                new Address
                {
                    AddressId = 3,
                    StreetAddress = "789 Fake Boulevard",
                    City = "Fakeville",
                    State = "FV",
                    PostalCode = "11122",
                    Country = "Fake Country"
                });

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    CompanyId = 1,
                    Name = "Fake Company 1",
                    CeoUserId = 1, // Assuming User1 is the CEO
                    AddressId = 1, // Assuming Address1 is the company address
                    ContactEmail = "fake1@example.com",
                    ContactPhoneNumber = "1234567890",
                    AddedById = 1, // Assuming User1 added this company
                    Logo = null
                },
                new Company
                {
                    CompanyId = 2,
                    Name = "Fake Company 2",
                    CeoUserId = 2, // Assuming User2 is the CEO
                    AddressId = 2, // Assuming Address2 is the company address
                    ContactEmail = "fake2@example.com",
                    ContactPhoneNumber = "0987654321",
                    AddedById = 2, // Assuming User2 added this company
                    Logo = null
                },
                new Company
                {
                    CompanyId = 3,
                    Name = "Fake Company 3",
                    CeoUserId = 3, // Assuming User3 is the CEO
                    AddressId = 3, // Assuming Address3 is the company address
                    ContactEmail = "fake3@example.com",
                    ContactPhoneNumber = "1231231231",
                    AddedById = 3, // Assuming User3 added this company
                    Logo = null
                });

            modelBuilder.Entity<Tier>().HasData(
                new Tier
                {
                    TierId = 1,
                    Name = "Supervisor",
                    Duty = "Duty for Supervisor",
                    Created = DateTime.UtcNow
                },
                new Tier
                {
                    TierId = 2,
                    Name = "Employee",
                    Duty = "Duty for Employee",
                    Created = DateTime.UtcNow
                });

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    Username = "FakeEmployee1",
                    Role = "supervisor",
                    Email = "employee1@example.com",
                    PhoneNumber = "1234567890",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"),
                    FirstName = "First",
                    LastName = "Employee",
                    Gender = "Male",
                    Created = DateTime.UtcNow,
                    CompanyId = 1, // Assuming the company is the one you just created
                    TierId = 1, // Assuming the tier is the one you just created
                    LockedEnabled = false,
                    LoginAttempts = 0,
                    SupervisorId = null // First user has no supervisor
                },
                new Employee
                {
                    EmployeeId = 2,
                    Username = "FakeEmployee2",
                    Role = "employee",
                    Email = "employee2@example.com",
                    PhoneNumber = "0987654321",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"),
                    FirstName = "Second",
                    LastName = "Employee",
                    Gender = "Female",
                    Created = DateTime.UtcNow,
                    CompanyId = 2, // Assuming the company is the one you just created
                    TierId = 2, // Assuming the tier is the one you just created
                    LockedEnabled = false,
                    LoginAttempts = 0,
                    SupervisorId = 1 // Second user's supervisor is the first user
                });

            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    ProjectId = 1,
                    Name = "Project 1",
                    Description = "Description for Project 1",
                    Created = DateTime.UtcNow,
                    ProjectCreatorId = 1, // Assuming Employee1 is the project creator
                    CompanyId = 1, // Assuming the company is the one you just created
                    Priority = 1,
                    ExpectedDeliveryDate = DateTime.UtcNow.AddDays(30),
                    Lifecycle = "Planning",
                    LatestTaskCreation = DateTime.UtcNow
                },
                new Project
                {
                    ProjectId = 2,
                    Name = "Project 2",
                    Description = "Description for Project 2",
                    Created = DateTime.UtcNow,
                    ProjectCreatorId = 1, // Assuming Employee1 is the project creator
                    CompanyId = 2, // Assuming the company is the one you just created
                    Priority = 2,
                    ExpectedDeliveryDate = DateTime.UtcNow.AddDays(60),
                    Lifecycle = "Development",
                    LatestTaskCreation = DateTime.UtcNow
                },
                new Project
                {
                    ProjectId = 3,
                    Name = "Project 3",
                    Description = "Description for Project 3",
                    Created = DateTime.UtcNow,
                    ProjectCreatorId = 1, // Assuming Employee1 is the project creator
                    CompanyId = 3, // Assuming the company is the one you just created
                    Priority = 3,
                    ExpectedDeliveryDate = DateTime.UtcNow.AddDays(90),
                    Lifecycle = "Testing",
                    LatestTaskCreation = DateTime.UtcNow
                });

            modelBuilder.Entity<Models.Task>().HasData(
                // Tasks for Project 1
                new Models.Task
                {
                    TaskId = 1,
                    Name = "Task 1",
                    Description = "Description for Task 1",
                    Created = DateTime.UtcNow,
                    TaskCreatorId = 1, // Assuming Employee1 is the task creator
                    ProjectId = 1 // This task belongs to Project 1
                },
                new Models.Task
                {
                    TaskId = 2,
                    Name = "Task 2",
                    Description = "Description for Task 2",
                    Created = DateTime.UtcNow,
                    TaskCreatorId = 1, // Assuming Employee1 is the task creator
                    ProjectId = 1 // This task belongs to Project 1
                },
                new Models.Task
                {
                    TaskId = 3,
                    Name = "Task 3",
                    Description = "Description for Task 3",
                    Created = DateTime.UtcNow,
                    TaskCreatorId = 1, // Assuming Employee1 is the task creator
                    ProjectId = 1 // This task belongs to Project 1
                },
                // Tasks for Project 2
                new Models.Task
                {
                    TaskId = 4,
                    Name = "Task 4",
                    Description = "Description for Task 4",
                    Created = DateTime.UtcNow,
                    TaskCreatorId = 1, // Assuming Employee1 is the task creator
                    ProjectId = 2 // This task belongs to Project 2
                },
                new Models.Task
                {
                    TaskId = 5,
                    Name = "Task 5",
                    Description = "Description for Task 5",
                    Created = DateTime.UtcNow,
                    TaskCreatorId = 1, // Assuming Employee1 is the task creator
                    ProjectId = 2 // This task belongs to Project 2
                },
                new Models.Task
                {
                    TaskId = 6,
                    Name = "Task 6",
                    Description = "Description for Task 6",
                    Created = DateTime.UtcNow,
                    TaskCreatorId = 1, // Assuming Employee1 is the task creator
                    ProjectId = 2 // This task belongs to Project 2
                },
                // Tasks for Project 3
                new Models.Task
                {
                    TaskId = 7,
                    Name = "Task 7",
                    Description = "Description for Task 7",
                    Created = DateTime.UtcNow,
                    TaskCreatorId = 1, // Assuming Employee1 is the task creator
                    ProjectId = 3 // This task belongs to Project 3
                },
                new Models.Task
                {
                    TaskId = 8,
                    Name = "Task 8",
                    Description = "Description for Task 8",
                    Created = DateTime.UtcNow,
                    TaskCreatorId = 1, // Assuming Employee1 is the task creator
                    ProjectId = 3 // This task belongs to Project 3
                },
                new Models.Task
                {
                    TaskId = 9,
                    Name = "Task 9",
                    Description = "Description for Task 9",
                    Created = DateTime.UtcNow,
                    TaskCreatorId = 1, // Assuming Employee1 is the task creator
                    ProjectId = 3 // This task belongs to Project 3
                });

            modelBuilder.Entity<Issue>().HasData(
                // Issues for Task 1
                new Issue
                {
                    IssueId = 1,
                    Name = "Issue 1",
                    Description = "Description for Issue 1",
                    Created = DateTime.UtcNow,
                    StartedWorking = DateTime.UtcNow,
                    Finished = DateTime.UtcNow.AddDays(7),
                    IssueCreatorId = 1, // Assuming Employee1 is the issue creator
                    TaskId = 1 // This issue belongs to Task 1
                },
                // Issues for Task 2
                new Issue
                {
                    IssueId = 2,
                    Name = "Issue 2",
                    Description = "Description for Issue 2",
                    Created = DateTime.UtcNow,
                    StartedWorking = DateTime.UtcNow,
                    Finished = DateTime.UtcNow.AddDays(7),
                    IssueCreatorId = 1, // Assuming Employee1 is the issue creator
                    TaskId = 2 // This issue belongs to Task 2
                },
                // Issues for Task 4
                new Issue
                {
                    IssueId = 3,
                    Name = "Issue 3",
                    Description = "Description for Issue 3",
                    Created = DateTime.UtcNow,
                    StartedWorking = DateTime.UtcNow,
                    Finished = DateTime.UtcNow.AddDays(7),
                    IssueCreatorId = 1, // Assuming Employee1 is the issue creator
                    TaskId = 4 // This issue belongs to Task 4
                },
                // Issues for Task 6
                new Issue
                {
                    IssueId = 4,
                    Name = "Issue 4",
                    Description = "Description for Issue 4",
                    Created = DateTime.UtcNow,
                    StartedWorking = DateTime.UtcNow,
                    Finished = DateTime.UtcNow.AddDays(7),
                    IssueCreatorId = 1, // Assuming Employee1 is the issue creator
                    TaskId = 6 // This issue belongs to Task 6
                },
                // Issues for Task 9
                new Issue
                {
                    IssueId = 5,
                    Name = "Issue 5",
                    Description = "Description for Issue 5",
                    Created = DateTime.UtcNow,
                    StartedWorking = DateTime.UtcNow,
                    Finished = DateTime.UtcNow.AddDays(7),
                    IssueCreatorId = 1, // Assuming Employee1 is the issue creator
                    TaskId = 9 // This issue belongs to Task 9
                });

            modelBuilder.Entity<EmployeeProject>().HasData(
                // Assign Employee 2 to Project 1
                new EmployeeProject
                {
                    RelationId = 1,
                    EmployeeId = 2,
                    ProjectId = 1
                },
                // Assign Employee 2 to Project 2
                new EmployeeProject
                {
                    RelationId = 2,
                    EmployeeId = 2,
                    ProjectId = 2
                },
                // Assign Employee 2 to Project 3
                new EmployeeProject
                {
                    RelationId = 3,
                    EmployeeId = 2,
                    ProjectId = 3
                });
            modelBuilder.Entity<EmployeeTask>().HasData(
                // Assign Task 4 to Employee 2
                new EmployeeTask
                {
                    RelationId = 1,
                    EmployeeId = 2,
                    TaskId = 4
                },
                // Assign Task 5 to Employee 2
                new EmployeeTask
                {
                    RelationId = 2,
                    EmployeeId = 2,
                    TaskId = 5
                },
                // Assign Task 6 to Employee 2
                new EmployeeTask
                {
                    RelationId = 3,
                    EmployeeId = 2,
                    TaskId = 6
                });
            modelBuilder.Entity<EmployeeIssue>().HasData(
                // Assign Issue 4 to Employee 2
                new EmployeeIssue
                {
                    RelationId = 1,
                    EmployeeId = 2,
                    IssueId = 4
                },
                // Assign Issue 5 to Employee 2
                new EmployeeIssue
                {
                    RelationId = 2,
                    EmployeeId = 2,
                    IssueId = 5
                });
        }
    }
}
