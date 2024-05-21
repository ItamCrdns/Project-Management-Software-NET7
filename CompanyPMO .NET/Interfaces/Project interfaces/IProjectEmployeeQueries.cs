using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces.Project_interfaces
{
    public interface IProjectEmployeeQueries
    {
        // Fully implemented in Employee Controller
        Task<DataCountPages<ProjectDto>> GetProjectsByEmployeeUsername(string username, FilterParams filterParams);
        Task<DataCountPages<ProjectShowcaseDto>> GetProjectsShowcaseByEmployeeUsername(string username, int page, int pageSize);
    }
}
