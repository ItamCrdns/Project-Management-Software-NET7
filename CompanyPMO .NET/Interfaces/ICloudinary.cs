namespace CompanyPMO_.NET.Interfaces
{
    public interface ICloudinary
    {
        Task<(string imageUrl, string publicId)> UploadToCloudinary(IFormFile file, int width, int height);
    }
}
