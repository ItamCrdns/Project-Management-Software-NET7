using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IImage
    {
        Task<(string imageUrl, string publicId)> UploadToCloudinary(IFormFile file, int width, int height);
    }
}
