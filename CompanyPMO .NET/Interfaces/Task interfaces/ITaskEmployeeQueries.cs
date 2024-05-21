using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Task_interfaces
{
    public interface ITaskEmployeeQueries
    {
        // Fully implemented in Employee Controller
        Task<DataCountPages<TaskDto>> GetTasksByEmployeeUsername(string username, int page, int pageSize);
        Task<DataCountPages<TaskShowcaseDto>> GetTasksShowcaseByEmployeeUsername(string username, int page, int pageSize);
    }
}
