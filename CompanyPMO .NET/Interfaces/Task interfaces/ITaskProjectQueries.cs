using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces.Task_interfaces
{
    public interface ITaskProjectQueries
    {
        // Fully implemented in Project Controller
        Task<EntityParticipantOrOwnerDTO<TaskDto>> GetTaskById(int taskId, int projectId, int userId);
        Task<DataCountPages<TaskShowcaseDto>> GetTasksShowcaseByProjectId(int projectId, int page, int pageSize);
        Task<DataCountPages<TaskDto>> GetTasksByProjectId(int projectId, FilterParams filterParams);
    }
}
