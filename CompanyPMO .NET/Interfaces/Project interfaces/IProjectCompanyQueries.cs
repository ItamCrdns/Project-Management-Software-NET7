using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces.Project_interfaces
{
    public interface IProjectCompanyQueries
    {
        // Fully implemented in Company controller
        Task<DataCountPages<ProjectDto>> GetProjectsByCompanyName(int companyId, FilterParams filterParams);
    }
}
