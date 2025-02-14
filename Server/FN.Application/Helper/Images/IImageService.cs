﻿using Microsoft.AspNetCore.Http;

namespace FN.Application.Helper.Images
{
    public interface IImageService
    {
        string GenerateId();
        Task<string?> UploadImage(IFormFile file, string publicId, string folderName);
    }
}
