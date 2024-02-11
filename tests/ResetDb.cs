using CompanyPMO_.NET.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class ResetDb
    {
        public static async Task Reset(ApplicationDbContext dbContext)
        {
            dbContext.Users.RemoveRange(await dbContext.Users.ToListAsync());
            dbContext.Addresses.RemoveRange(await dbContext.Addresses.ToListAsync());
            dbContext.Employees.RemoveRange(await dbContext.Employees.ToListAsync());
            dbContext.Tiers.RemoveRange(await dbContext.Tiers.ToListAsync());
            dbContext.Companies.RemoveRange(await dbContext.Companies.ToListAsync());
            dbContext.Projects.RemoveRange(await dbContext.Projects.ToListAsync());
            dbContext.EmployeeProjects.RemoveRange(await dbContext.EmployeeProjects.ToListAsync());
            dbContext.Tasks.RemoveRange(await dbContext.Tasks.ToListAsync());
            dbContext.Issues.RemoveRange(await dbContext.Issues.ToListAsync());
            dbContext.EmployeeTasks.RemoveRange(await dbContext.EmployeeTasks.ToListAsync());
            dbContext.EmployeeIssues.RemoveRange(await dbContext.EmployeeIssues.ToListAsync());

            await dbContext.SaveChangesAsync();
        }
    }
}
