using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CompanyPMO_.NET.Interfaces;

namespace CompanyPMO_.NET.Services
{
    public class CloudinaryService : ICloudinary
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<(string imageUrl, string publicId)> UploadToCloudinary(IFormFile file, int width, int height)
        {
            if (file is not null && file.Length > 0)
            {
                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = true,
                    Transformation = new Transformation().Quality(50).FetchFormat("webp")
                };

                // If width and height arguements are given crop the image accordingly
                if (width > 0 && height > 0)
                {
                    uploadParams.Transformation = new Transformation().Width(width).Height(height).Crop("fill");
                }

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error is not null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                return (uploadResult.SecureUrl.ToString(), uploadResult.PublicId);
            }
            return (null, null);
        }
    }
}
