using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;
using Task = System.Threading.Tasks.Task;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IWorkload
    {
        Task SpUpdateEmployeeAssignedProjectsCount(int[] employees);
        Task SpUpdateEmployeeWorkloadAssignedTasksAndIssues(int[] employees);
        Task SpUpdateEmployeeCompletedProjects(int[] employees);
    }
}
