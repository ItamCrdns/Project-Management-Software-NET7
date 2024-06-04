using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Interfaces.Project_interfaces
{
    public interface IProjectPicture
    {
        Task<OperationResult> AddPicturesToProject(int projectId, int employeeId, List<IFormFile> pictures);
    }
}
