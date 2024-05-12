using CompanyPMO_.NET.Models;
using Task = CompanyPMO_.NET.Models.Task;

namespace CompanyPMO_.NET.Data
{
    public static class Seed
    {
        public static void SeedData(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.AddRange(new List<User>
                {
                    new() {
                        UserId = 1,
                        Username = "JohnDoe",
                        FirstName = "John",
                        LastName = "Doe",
                        Gender = "Male",
                        Created = DateTime.UtcNow,
                        LastLogin = DateTime.UtcNow
                    },
                    new()
                    {
                        UserId = 2,
                        Username = "JaneDoe",
                        FirstName = "Jane",
                        LastName = "Doe",
                        Gender = "Female",
                        Created = DateTime.UtcNow,
                        LastLogin = DateTime.UtcNow
                    }
                });

                context.SaveChanges();
            }

            if (!context.Addresses.Any())
            {
                context.Addresses.AddRange(new List<Address>
                {
                    new()
                    {
                        AddressId = 1,
                        StreetAddress = "123 Fake Street",
                        City = "Faketown",
                        State = "FS",
                        PostalCode = "12345",
                        Country = "Fake Country"
                    },
                    new()
                    {
                        AddressId = 2,
                        StreetAddress = "456 Fake Avenue",
                        City = "Fakecity",
                        State = "FC",
                        PostalCode = "67890",
                        Country = "Fake Country"
                    },
                    new()
                    {
                        AddressId = 3,
                        StreetAddress = "789 Fake Boulevard",
                        City = "Fakeville",
                        State = "FV",
                        PostalCode = "11122",
                        Country = "Fake Country"
                    }
                });

                context.SaveChanges();
            }

            if (!context.Companies.Any())
            {
                context.Companies.AddRange(new List<Company>
                {
                    new()
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
                    new()
                    {
                        CompanyId = 2,
                        Name = "Fake Company 2",
                        CeoUserId = 2, // Assuming User2 is the CEO
                        AddressId = 2, // Assuming Address2 is the company address
                        ContactEmail = "fake2@example.com",
                        ContactPhoneNumber = "0987654321",
                        AddedById = 2, // Assuming User2 added this company
                        Logo = null
                    }
                });

                context.SaveChanges();
            }

            if  (!context.Tiers.Any())
            {
                context.Tiers.AddRange(new List<Tier>
                {
                    new()
                    {
                        TierId = 1,
                        Name = "Supervisor",
                        Duty = "Supervisor duties here",
                        Created = DateTime.UtcNow
                    },
                    new()
                    {
                        TierId = 2,
                        Name = "Employee",
                        Duty = "Employee duties here",
                        Created = DateTime.UtcNow
                    },
                    new()
                    {
                        TierId = 3,
                        Name = "Trainee",
                        Duty = "Trainee duties here",
                        Created = DateTime.UtcNow
                    },
                    new()
                    {
                        TierId = 4,
                        Name = "Intern",
                        Duty = "Intern duties here",
                        Created = DateTime.UtcNow
                    }
                });

                context.SaveChanges();
            }

            if (!context.Employees.Any())
            {
                context.Employees.AddRange(new List<Employee>
                {
                    new()
                    {
                        EmployeeId = 1,
                        Username = "FakeEmployee1",
                        Email = "fakeemploye1@example.com",
                        PhoneNumber = "1234567890",
                        Password = BCrypt.Net.BCrypt.HashPassword("password"),
                        FirstName = "First",
                        LastName = "Employee",
                        Gender = "Male",
                        Created = DateTime.UtcNow,
                        CompanyId = 1,
                        TierId = 1, // Supervisor
                        LockedEnabled = false,
                        LoginAttempts = 0,
                        SupervisorId = null // First user has no supervisor
                    },
                    new()
                    {
                        EmployeeId = 2,
                        Username = "FakeEmployee2",
                        Email = "fakeemploye2@example.com",
                        PhoneNumber = "1234567890",
                        Password = BCrypt.Net.BCrypt.HashPassword("password"),
                        FirstName = "Second",
                        LastName = "Employee",
                        Gender = "Male",
                        Created = DateTime.UtcNow,
                        CompanyId = 1,
                        TierId = 2, // Employee
                        LockedEnabled = false,
                        LoginAttempts = 0,
                        SupervisorId = 1
                    },
                    new()
                    {
                        EmployeeId = 3,
                        Username = "FakeEmployee3",
                        Email = "fakeemploye3@example.com",
                        PhoneNumber = "1234567890",
                        Password = BCrypt.Net.BCrypt.HashPassword("password"),
                        FirstName = "Third",
                        LastName = "Employee",
                        Gender = "Female",
                        Created = DateTime.UtcNow,
                        CompanyId = 2,
                        TierId = 3, // Trainee
                        LockedEnabled = false,
                        LoginAttempts = 0,
                        SupervisorId = 1
                    },
                    new()
                    {
                        EmployeeId = 4,
                        Username = "FakeEmployee4",
                        Email = "fakeemploye4@example.com",
                        PhoneNumber = "1234567890",
                        Password = BCrypt.Net.BCrypt.HashPassword("password"),
                        FirstName = "Fourth",
                        LastName = "Employee",
                        Gender = "Female",
                        Created = DateTime.UtcNow,
                        CompanyId = 2,
                        TierId = 4, // Intern
                        LockedEnabled = false,
                        LoginAttempts = 0,
                        SupervisorId = 1
                    }
                });

                context.SaveChanges();
            }

            if (!context.Workload.Any())
            {
                context.Workload.AddRange(new List<Workload>
                {
                    // Since employee-workload its a one-to-one relationship, we need to create a workload for each employee
                    new()
                    {
                        WorkloadId = 1,
                        WorkloadSum = "Not specified"
                    },
                    new()
                    {
                        WorkloadId = 2,
                        WorkloadSum = "Not specified"
                    },
                    new()
                    {
                        WorkloadId = 3,
                        WorkloadSum = "Not specified"
                    },
                    new()
                    {
                        WorkloadId = 4,
                        WorkloadSum = "Not specified"
                    }
                });

                context.SaveChanges();
            }

            if (!context.Projects.Any())
            {
                context.Projects.AddRange(new List<Project>
                {
                    new()
                    {
                        ProjectId = 1,
                        Name = "Project 1",
                        Description = "Description for Project 1",
                        Created = DateTime.UtcNow,
                        ProjectCreatorId = 1,
                        CompanyId = 1,
                        Priority = 1,
                        ExpectedDeliveryDate = DateTime.UtcNow.AddDays(30),
                        Lifecycle = "Planning",
                        LatestTaskCreation = DateTime.UtcNow
                    },
                    new()
                    {
                        ProjectId = 2,
                        Name = "Project 2",
                        Description = "Description for Project 2",
                        Created = DateTime.UtcNow,
                        ProjectCreatorId = 1,
                        CompanyId = 1,
                        Priority = 2,
                        ExpectedDeliveryDate = DateTime.UtcNow.AddDays(60),
                        Lifecycle = "Development",
                        LatestTaskCreation = DateTime.UtcNow
                    },
                    new()
                    {
                        ProjectId = 3,
                        Name = "Project 3",
                        Description = "Description for Project 3",
                        Created = DateTime.UtcNow,
                        ProjectCreatorId = 2,
                        CompanyId = 2,
                        Priority = 1,
                        ExpectedDeliveryDate = DateTime.UtcNow.AddDays(90),
                        Lifecycle = "Testing",
                        LatestTaskCreation = DateTime.UtcNow
                    },
                });

                context.SaveChanges();
            }

            if (!context.Tasks.Any())
            {
                context.Tasks.AddRange(new List<Task>
                {
                    // Tasks for project 1
                    new()
                    {
                        TaskId = 1,
                        Name = "Task 1",
                        Description = "Description for Task 1",
                        Created = DateTime.UtcNow,
                        TaskCreatorId = 1, // Assuming Employee1 is the task creator
                        ProjectId = 1 // This task belongs to Project 1
                    },
                    new()
                    {
                        TaskId = 2,
                        Name = "Task 2",
                        Description = "Description for Task 2",
                        Created = DateTime.UtcNow,
                        TaskCreatorId = 1, // Assuming Employee1 is the task creator
                        ProjectId = 1 // This task belongs to Project 1
                    },
                    // Tasks for project 2
                    new()
                    {
                        TaskId = 3,
                        Name = "Task 3",
                        Description = "Description for Task 3",
                        Created = DateTime.UtcNow,
                        TaskCreatorId = 1, // Assuming Employee1 is the task creator
                        ProjectId = 2 // This task belongs to Project 2
                    },
                    new()
                    {
                        TaskId = 4,
                        Name = "Task 4",
                        Description = "Description for Task 4",
                        Created = DateTime.UtcNow,
                        TaskCreatorId = 1, // Assuming Employee1 is the task creator
                        ProjectId = 2 // This task belongs to Project 2
                    },
                    // Tasks for project 3
                    new()
                    {
                        TaskId = 5,
                        Name = "Task 5",
                        Description = "Description for Task 5",
                        Created = DateTime.UtcNow,
                        TaskCreatorId = 2, // Assuming Employee2 is the task creator
                        ProjectId = 3 // This task belongs to Project 3
                    },
                    new()
                    {
                        TaskId = 6,
                        Name = "Task 6",
                        Description = "Description for Task 6",
                        Created = DateTime.UtcNow,
                        TaskCreatorId = 2, // Assuming Employee2 is the task creator
                        ProjectId = 3 // This task belongs to Project 3
                    }
                });

                context.SaveChanges();
            }

            if (!context.Issues.Any())
            {
                context.Issues.AddRange(new List<Issue>
                {
                    // Issue for Task 1
                    new()
                    {
                        IssueId = 1,
                        Name = "Issue 1",
                        Description = "Description for Issue 1",
                        Created = DateTime.UtcNow,
                        StartedWorking = DateTime.UtcNow,
                        Finished = DateTime.UtcNow.AddDays(7),
                        IssueCreatorId = 1,
                        TaskId = 1
                    },
                    // Issue for Task 2
                    new()
                    {
                        IssueId = 2,
                        Name = "Issue 2",
                        Description = "Description for Issue 2",
                        Created = DateTime.UtcNow,
                        StartedWorking = DateTime.UtcNow,
                        Finished = DateTime.UtcNow.AddDays(7),
                        IssueCreatorId = 1,
                        TaskId = 2
                    },
                    // Issue for Task 3
                    new()
                    {
                        IssueId = 3,
                        Name = "Issue 3",
                        Description = "Description for Issue 3",
                        Created = DateTime.UtcNow,
                        StartedWorking = DateTime.UtcNow,
                        Finished = DateTime.UtcNow.AddDays(7),
                        IssueCreatorId = 1,
                        TaskId = 3
                    },
                });

                context.SaveChanges();
            }

            if (!context.EmployeeProjects.Any())
            {
                context.EmployeeProjects.AddRange(new List<EmployeeProject>
                {
                    // Assign employees 2, 3 and 4 to project 1
                    new()
                    {
                        RelationId = 1,
                        EmployeeId = 2,
                        ProjectId = 1
                    },
                    new()
                    {
                        RelationId = 2,
                        EmployeeId = 3,
                        ProjectId = 1
                    },
                    new()
                    {
                        RelationId = 3,
                        EmployeeId = 4,
                        ProjectId = 1
                    },
                    // Assign employees 2, 3 and 4 to project 2
                    new()
                    {
                        RelationId = 4,
                        EmployeeId = 2,
                        ProjectId = 2
                    },
                    new()
                    {
                        RelationId = 5,
                        EmployeeId = 3,
                        ProjectId = 2
                    },
                    new()
                    {
                        RelationId = 6,
                        EmployeeId = 4,
                        ProjectId = 2
                    },
                    // Assign employees 2, 3 and 4 to project 3
                    new()
                    {
                        RelationId = 7,
                        EmployeeId = 2,
                        ProjectId = 3
                    },
                    new()
                    {
                        RelationId = 8,
                        EmployeeId = 3,
                        ProjectId = 3
                    },
                    new()
                    {
                        RelationId = 9,
                        EmployeeId = 4,
                        ProjectId = 3
                    }
                }); ;

                context.SaveChanges();
            }

            if (!context.EmployeeTasks.Any())
            {
                context.EmployeeTasks.AddRange(new List<EmployeeTask>
                {
                    // Assign employees 2, 3 and 4 to task 1
                    new()
                    {
                        RelationId = 1,
                        EmployeeId = 2,
                        TaskId = 1
                    },
                    new()
                    {
                        RelationId = 2,
                        EmployeeId = 3,
                        TaskId = 1
                    },
                    new()
                    {
                        RelationId = 3,
                        EmployeeId = 4,
                        TaskId = 1
                    },
                    // Assign employees 2, 3 and 4 to task 2
                    new()
                    {
                        RelationId = 4,
                        EmployeeId = 2,
                        TaskId = 2
                    },
                    new()
                    {
                        RelationId = 5,
                        EmployeeId = 3,
                        TaskId = 2
                    },
                    new()
                    {
                        RelationId = 6,
                        EmployeeId = 4,
                        TaskId = 2
                    },
                    // Assign employees 2, 3 and 4 to task 3
                    new()
                    {
                        RelationId = 7,
                        EmployeeId = 2,
                        TaskId = 3
                    },
                    new()
                    {
                        RelationId = 8,
                        EmployeeId = 3,
                        TaskId = 3
                    },
                    new()
                    {
                        RelationId = 9,
                        EmployeeId = 4,
                        TaskId = 3
                    },
                    // Assign employees 2 and 3 to task 4
                    new()
                    {
                        RelationId = 10,
                        EmployeeId = 2,
                        TaskId = 4
                    },
                    new()
                    {
                        RelationId = 11,
                        EmployeeId = 3,
                        TaskId = 4
                    },
                    // Assign employees 2 and 3 to task 5
                    new()
                    {
                        RelationId = 12,
                        EmployeeId = 2,
                        TaskId = 5
                    },
                    new()
                    {
                        RelationId = 13,
                        EmployeeId = 3,
                        TaskId = 5
                    },
                });

                context.SaveChanges();
            }

            if (!context.EmployeeIssues.Any())
            {
                context.EmployeeIssues.AddRange(new List<EmployeeIssue>
                {
                    // Assign employees 2 and 3 to issue 1
                    new()
                    {
                        RelationId = 1,
                        EmployeeId = 2,
                        IssueId = 1
                    },
                    new()
                    {
                        RelationId = 2,
                        EmployeeId = 3,
                        IssueId = 1
                    },

                    // Assign employee 4 to issue 2
                    new()
                    {
                        RelationId = 3,
                        EmployeeId = 4,
                        IssueId = 2
                    }
                });

                context.SaveChanges();
            }
        }
    }
}
