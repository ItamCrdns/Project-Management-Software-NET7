using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Task = System.Threading.Tasks.Task; // Never naming a class Task ever again

namespace CompanyPMO_.NET.Repository
{
    public class WorkloadRepository : IWorkload
    {
        private readonly ApplicationDbContext _context;
        public WorkloadRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SpUpdateEmployeeAssignedProjectsCount(int[] employees)
        {
            // Calls the stored procedure that updates the employee's assigned projects count
            var parameter = new NpgsqlParameter("@EmployeeId", NpgsqlDbType.Array | NpgsqlDbType.Integer)
            {
                Value = employees
            };

            await _context.Database.ExecuteSqlRawAsync("CALL sp_update_employee_assigned_projects(@EmployeeId)", parameter);
        }

        public Task SpUpdateEmployeeCompletedProjects(int[] employees)
        {
            throw new NotImplementedException();
        }

        public async Task SpUpdateEmployeeWorkloadAssignedTasksAndIssues(int[] employees)
        {
            // Calls the stored procedure that updates the employee's workload assigned tasks and issues
            var parameter = new NpgsqlParameter("@EmployeeId", NpgsqlDbType.Array | NpgsqlDbType.Integer)
            {
                Value = employees
            };

            await _context.Database.ExecuteSqlRawAsync("CALL sp_update_employee_workload_assigned_tasks_and_issues(@EmployeeId)", parameter);
        }
    }
}
