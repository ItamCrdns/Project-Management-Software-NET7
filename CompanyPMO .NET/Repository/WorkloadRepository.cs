using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class WorkloadRepository : IWorkload
    {
        private readonly ApplicationDbContext _context;
        public WorkloadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> UpdateEmployeeAssignedProjectsCount(int[] employees)
        {
            List<Workload> workloadsToUpdate = new();

            foreach (int employee in employees.Distinct())
            {
                int ongoingProjectsCount = await _context.Projects
                    .Where(x => x.Employees.Any(e => e.EmployeeId == employee) && x.Finished == null)
                    .CountAsync();

                var workload = await _context.Workload.FirstOrDefaultAsync(x => x.WorkloadId == employee);

                if (workload != null)
                {
                    workload.AssignedProjects = ongoingProjectsCount;
                    workloadsToUpdate.Add(workload);
                }
            }

            if (workloadsToUpdate.Count > 0)
            {
                _context.Workload.UpdateRange(workloadsToUpdate);

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

            return new OperationResult { Success = false, Message = "No workloads to update" };
        }

        public async Task<OperationResult> UpdateEmployeeCompletedProjects(int[] employees)
        {
            List<Workload> workloadsToUpdate = new();

            foreach (int employee in employees.Distinct())
            {
                int finishedProjectsCount = await _context.Projects
                    .Where(x => x.Employees.Any(e => e.EmployeeId == employee) && x.Finished != null)
                    .CountAsync();

                var workload = await _context.Workload.FirstOrDefaultAsync(x => x.WorkloadId == employee);

                if (workload != null)
                {
                    workload.CompletedProjects = finishedProjectsCount;
                    workloadsToUpdate.Add(workload);
                }
            }

            if (workloadsToUpdate.Count > 0)
            {
                _context.Workload.UpdateRange(workloadsToUpdate);

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

            return new OperationResult { Success = false, Message = "No workloads to update" };
        }

        public async Task<OperationResult<List<WorkloadDto>>> UpdateEmployeeWorkloadAssignedTasksAndIssues(int[] employees)
        {
            List<Workload> workloadsToUpdate = new();

            foreach (int employee in employees.Distinct())
            {
                // Count tasks employee is working on excluding finished
                int ongoingTasksCount = await _context.Tasks
                    .Where(x => x.Employees.Any(e => e.EmployeeId == employee) && x.Finished == null)
                    .CountAsync();

                // Count issues employee is working on excluding finished
                int ongoingIssuesCount = await _context.Issues
                    .Where(x => x.Employees.Any(e => e.EmployeeId == employee) && x.Finished == null)
                    .CountAsync();

                // * To determine the workload:
                // Each task count as 2 points and each issues as 1 point

                int totalCount = (ongoingTasksCount * 2) + ongoingIssuesCount;

                // Determine the workload string. Ex: "High", "Medium", "Low"
                string workloadSum = totalCount switch
                {
                    > 20 => "Very High",
                    > 10 => "High",
                    > 5 => "Medium",
                    > 2 => "Low",
                    _ => "None"
                };

                Workload workload = await _context.Workload.FirstOrDefaultAsync(x => x.WorkloadId == employee);

                if (workload != null)
                {
                    workload.WorkloadSum = workloadSum;
                    workload.AssignedTasks = ongoingTasksCount;
                    workload.AssignedIssues = ongoingIssuesCount;

                    workloadsToUpdate.Add(workload);
                }
            }

            if (workloadsToUpdate.Count > 0)
            {
                _context.Workload.UpdateRange(workloadsToUpdate);

                int rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return new OperationResult<List<WorkloadDto>>
                    {
                        Success = true,
                        Message = "Employee workload assigned tasks and issues updated successfully.",
                        Data = workloadsToUpdate.Select(x => new WorkloadDto
                        {
                            WorkloadId = x.WorkloadId,
                            WorkloadSum = x.WorkloadSum,
                        }).ToList()
                    };
                }
                else
                {
                    return new OperationResult<List<WorkloadDto>> { Success = false, Message = "Something went wrong" };
                }
            }

            return new OperationResult<List<WorkloadDto>> { Success = false, Message = "No workloads to update" };
        }
    }
}
