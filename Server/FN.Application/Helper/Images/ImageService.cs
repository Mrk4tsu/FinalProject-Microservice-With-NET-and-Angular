using Microsoft.AspNetCore.Http;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;

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
        
        public string GenerateId() => Guid.NewGuid().ToString().Substring(4, 4);

        public async Task<bool> DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams($"{Root}/{publicId}")
            {
                ResourceType = ResourceType.Image,
            };
            var result = await _cloudinary.DestroyAsync(deleteParams);
            if (result.StatusCode == HttpStatusCode.OK) return true;
            return false;
        }
        public async Task<bool> DeleteFolderImage(string folderName)
        {
            var result = await _cloudinary.DeleteFolderAsync($"{Root}/{folderName}");
            Console.WriteLine(result.StatusCode.ToString());
            if (result.StatusCode != HttpStatusCode.OK)
                return false;
            return true;
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
                if (uploadResult.StatusCode == HttpStatusCode.OK)
                    return uploadResult.SecureUrl.AbsoluteUri;
                return null;
            }
            return null;
        }
        public async Task<List<string>> UploadImages(List<IFormFile> files, string folderName)
        {
            var uploadResults = new List<string>();
            foreach (var image in files)
            {
                var uploadParam = new ImageUploadParams
                {
                    Transformation = new Transformation().Quality(50).Chain(),
                    File = new FileDescription(image.FileName, image.OpenReadStream()),
                    Folder = $"{Root}/{folderName}",
                    Overwrite = true
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParam);
                uploadResults.Add(uploadResult.SecureUrl.AbsoluteUri);
            }
            return uploadResults;
        }
    }
}
