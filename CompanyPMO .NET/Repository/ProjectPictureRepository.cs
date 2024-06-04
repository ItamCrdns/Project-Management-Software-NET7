using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Project_interfaces;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Repository
{
    public class ProjectPictureRepository : IProjectPicture
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinary _cloudinaryService;
        public ProjectPictureRepository(ApplicationDbContext context, ICloudinary cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<OperationResult> AddPicturesToProject(int projectId, int employeeId, List<IFormFile> pictures)
        {
            var project = await _context.Projects.FindAsync(projectId);

            if (project is null)
            {
                return new OperationResult
                {
                    Message = "Project not found",
                    Success = false
                };
            }

            var projectPictures = new List<ProjectPicture>();

            foreach (var picture in pictures)
            {
                var (imageUrl, publicId) = await _cloudinaryService.UploadToCloudinary(picture, 0, 0);

                if (imageUrl != null && publicId != null)
                {
                    var projectPicture = new ProjectPicture
                    {
                        ProjectId = projectId,
                        ImageUrl = imageUrl,
                        CloudinaryPublicId = publicId,
                        Created = DateTime.UtcNow,
                        EmployeeId = employeeId
                    };

                    projectPictures.Add(projectPicture);
                }
            }

            if (projectPictures.Count == 0)
            {
                return new OperationResult
                {
                    Message = "No pictures uploaded",
                    Success = false
                };
            }

            await _context.ProjectPictures.AddRangeAsync(projectPictures);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new OperationResult
                {
                    Message = "Pictures uploaded successfully",
                    Success = true
                };
            }

            return new OperationResult
            {
                Message = "Pictures upload failed",
                Success = false
            };
        }
    }
}
