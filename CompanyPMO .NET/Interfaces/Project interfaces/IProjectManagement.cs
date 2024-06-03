using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces.Project_interfaces
{
    public interface IProjectManagement
    {
        // PENDING: Implement AddImagesToExistingProject in Project Management Controller
        Task<OperationResult<int>> CreateProject(Project project, EmployeeDto supervisor, List<IFormFile>? images, int companyId, List<int>? employeeIds, bool shouldStartNow);
        Task<OperationResult> SetProjectsStartBulk(int[] projectIds, int employeeId);
        Task<OperationResult> SetProjectStart(int projectId, int employeeId);
        Task<OperationResult> SetProjectsFininishedBulk(int[] projectIds, int employeeId);
        Task<OperationResult> SetProjectFinished(int projectId, int employeeId);
    }
}
