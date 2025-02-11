using Microsoft.AspNetCore.Http;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace FN.Application.Helper.Images
{
    public class ImageService : IImageService
    {
        private const string Root = "public";
        private readonly Cloudinary _cloudinary;
        public ImageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }
        public async Task<string?> UploadImage(IFormFile file, string publicId, string folderName)
        {
            if (file != null && file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    Transformation = new Transformation().Quality(50).Chain(),
                    Format = "webp",
                    File = new FileDescription(file.FileName, stream),
                    PublicId = publicId,
                    Folder = $"{Root}/{folderName}",
                    Overwrite = true
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.AbsoluteUri;
            }
            return null;
        }






        public string GenerateId() => Guid.NewGuid().ToString().Substring(4, 4);

    }
}
