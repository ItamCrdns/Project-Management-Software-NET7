using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Workload_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class WorkloadRepository : IWorkload, IWorkloadTask, IWorkloadProject, IWorkloadIssue, IWorkloadEmployee, IWorkloadOverdues
    {
        private readonly ApplicationDbContext _context;
        public WorkloadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> CreateWorkloadEntityForEmployee(int employeeId)
        {
            bool employeeExists = await _context.Employees.AnyAsync(x => x.EmployeeId == employeeId);

            if (!employeeExists)
            {
                return new OperationResult { Success = false, Message = "Employee not found." };
            }

            bool workloadExists = await _context.Workload.AnyAsync(x => x.WorkloadId == employeeId);

            if (workloadExists)
            {
                return new OperationResult { Success = false, Message = "Workload entity already exists for this employee." };
            }

            var workload = new Workload
            {
                WorkloadId = employeeId,
                WorkloadSum = "None"
            };

            _context.Add(workload);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Workload entity created successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<Workload?> GetWorkloadByEmployee(string username)
        {
            return await _context.Workload
                .FirstOrDefaultAsync(x => x.Employee.Username == username);
        }

        public async Task<OperationResult> UpdateEmployeeCompletedProjects(int[] employees)
        {
            var workloads = await _context.Workload
                .Where(x => employees.Contains(x.WorkloadId))
                .ToListAsync();

            foreach (var workload in workloads)
            {
                workload.CompletedProjects = await _context.Projects
                    .Where(x => x.Employees.Any(e => e.EmployeeId == workload.WorkloadId) && x.Finished != null)
                    .CountAsync();
            }

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Employee completed projects count updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateEmployeeCompletedTasks(int[] employees)
        {
            var workloads = await _context.Workload
                .Where(x => employees.Contains(x.WorkloadId))
                .ToListAsync();

            foreach (var workload in workloads)
            {
                workload.CompletedTasks = await _context.Tasks
                    .Where(x => x.Employees.Any(e => e.EmployeeId == workload.WorkloadId) && x.Finished != null)
                    .CountAsync();
            }

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Employee completed tasks count updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateEmployeeCompletedIssues(int[] employees)
        {
            var workloads = await _context.Workload
                .Where(x => employees.Contains(x.WorkloadId))
                .ToListAsync();

            foreach (var workload in workloads)
            {
                workload.CompletedIssues = await _context.Issues
                    .Where(x => x.Employees.Any(e => e.EmployeeId == workload.WorkloadId) && x.Finished != null)
                    .CountAsync();
            }

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Employee completed issues count updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateEmployeeAssignedProjects(int[] employeeIds)
        {
            var workloads = await _context.Workload
                .Where(x => employeeIds.Contains(x.WorkloadId))
                .ToListAsync();

            foreach (var workload in workloads)
            {
                workload.AssignedProjects = await _context.Projects
                    .Where(x => x.Employees.Any(e => e.EmployeeId == workload.WorkloadId) && x.Finished == null)
                    .CountAsync();
            }

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Employee assigned projects count updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateEmployeeAssignedTasks(int[] employeeIds)
        {
            var workloads = await _context.Workload
                .Where(x => employeeIds.Contains(x.WorkloadId))
                .ToListAsync();

            foreach (var workload in workloads)
            {
                workload.AssignedTasks = await _context.Tasks
                    .Where(x => x.Employees.Any(e => e.EmployeeId == workload.WorkloadId) && x.Finished == null)
                    .CountAsync();
            }

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Employee assigned tasks count updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateEmployeeAssignedIssues(int[] employeeIds)
        {
            var workloads = await _context.Workload
                .Where(x => employeeIds.Contains(x.WorkloadId))
                .ToListAsync();

            foreach (var workload in workloads)
            {
                workload.AssignedIssues = await _context.Issues
                    .Where(x => x.Employees.Any(e => e.EmployeeId == workload.WorkloadId) && x.Finished == null)
                    .CountAsync();
            }

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Employee assigned issues count updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateOverdueProjects()
        {
            // retrieves a list of employee IDs for employees who are associated with projects that were expected to be delivered by now but are not yet finished
            List<int> employeeIds = await _context.Projects
                .Where(x => x.ExpectedDeliveryDate < DateTime.UtcNow && x.Finished == null)
                .SelectMany(x => x.Employees.Select(e => e.EmployeeId))
                .ToListAsync();

            var workloads = await _context.Workload
                .Where(x => employeeIds.Contains(x.WorkloadId))
                .ToListAsync();

            foreach (var workload in workloads)
            {
                workload.OverdueProjects = employeeIds.Count(id => id == workload.WorkloadId);
            }

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Overdue projects updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateOverdueTasks()
        {
            // retrieves a list of employee IDs for employees who are associated with tasks that were expected to be delivered by now but are not yet finished
            List<int> employeeIds = await _context.Tasks
                .Where(x => x.ExpectedDeliveryDate < DateTime.UtcNow && x.Finished == null)
                .SelectMany(x => x.Employees.Select(e => e.EmployeeId))
                .ToListAsync();

            var workloads = await _context.Workload
                .Where(x => employeeIds.Contains(x.WorkloadId))
                .ToListAsync();

            foreach (var workload in workloads)
            {
                workload.OverdueTasks = employeeIds.Count(id => id == workload.WorkloadId);
            }

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Overdue tasks updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateOverdueIssues()
        {
            // retrieves a list of employee IDs for employees who are associated with issues that were expected to be delivered by now but are not yet finished
            List<int> employeeIds = await _context.Issues
                .Where(x => x.ExpectedDeliveryDate < DateTime.UtcNow && x.Finished == null)
                .SelectMany(x => x.Employees.Select(e => e.EmployeeId))
                .ToListAsync();

            var workloads = await _context.Workload
                .Where(x => employeeIds.Contains(x.WorkloadId))
                .ToListAsync();

            foreach (var workload in workloads)
            {
                workload.OverdueIssues = employeeIds.Count(id => id == workload.WorkloadId);
            }

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Overdue issues updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateEmployeeCreatedTasks(int employeeId)
        {
            var workload = await _context.Workload.FindAsync(employeeId);

            if (workload == null)
            {
                return new OperationResult { Success = false, Message = "Workload entity not found." };
            }

            workload.CreatedTasks = await _context.Tasks
                .Where(x => x.TaskCreatorId == employeeId)
                .CountAsync();

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Employee created tasks count updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateEmployeeCreatedProjects(int employeeId)
        {
            var workload = await _context.Workload.FindAsync(employeeId);

            if (workload == null)
            {
                return new OperationResult { Success = false, Message = "Workload entity not found." };
            }

            workload.CreatedProjects = await _context.Projects
                .Where(x => x.ProjectCreatorId == employeeId)
                .CountAsync();

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Employee created projects count updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<OperationResult> UpdateEmployeeCreatedIssues(int employeeId)
        {
            var workload = await _context.Workload.FindAsync(employeeId);

            if (workload == null)
            {
                return new OperationResult { Success = false, Message = "Workload entity not found." };
            }

            workload.CreatedIssues = await _context.Issues
                .Where(x => x.IssueCreatorId == employeeId)
                .CountAsync();

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult { Success = true, Message = "Employee created issues count updated successfully." };
            }
            else
            {
                return new OperationResult { Success = false, Message = "Something went wrong" };
            }
        }
    }
}
