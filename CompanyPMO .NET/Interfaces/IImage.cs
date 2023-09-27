using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IImage
    {
        Task<(string imageUrl, string publicId)> UploadToCloudinary(IFormFile file, int width, int height);
        Task<List<Image>> AddImagesToNewEntity(ICollection<IFormFile> images, int entityId, string entityType);
        Task<IEnumerable<Image>> AddImagesToExistingCompany(int entity, List<IFormFile>? images, string entityType);
    }
}
