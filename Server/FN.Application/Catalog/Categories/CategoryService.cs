using FN.Application.Helper.Images;
using FN.DataAccess;
using FN.DataAccess.Entities;
using FN.Utilities;
using FN.ViewModel.Catalog.Categories;
using FN.ViewModel.Helper.API;
using Microsoft.EntityFrameworkCore;

namespace FN.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _db;
        private readonly IImageService _image;
        public CategoryService(AppDbContext db, IImageService image)
        {
            _db = db;
            _image = image;
        }
        public async Task<ApiResult<int>> Create(CategoryCreateUpdateRequest request)
        {
            if (StringHelper.IsNullOrEmpty(request.Other ?? string.Empty, request.Description ?? string.Empty, request.Name ?? string.Empty))
            {
                return new ApiErrorResult<int>("Vui lòng nhập đầy đủ thông tin");
            }
            var alias = StringHelper.GenerateSeoAlias(request.Name!);
            var thumbnail = await _image.UploadImage(request.Image!, alias, SetFolder(alias));
            if (string.IsNullOrEmpty(thumbnail))
                return new ApiErrorResult<int>("Không upload được ảnh");
            var newCategory = new Category
            {
                Name = request.Name!,
                SeoDescription = request.Description ?? "",
                Other = request.Other!,
                SeoAlias = alias,
                SeoImage = thumbnail,
                SeoKeyword = "mrkatsu, katsu, katsu website, gavl, gatapchoi",
                SeoTitle = $"MrKatsu - {request.Name}"
            };

            _db.Categories.Add(newCategory);
            await _db.SaveChangesAsync();
            return new ApiSuccessResult<int>(newCategory.Id);
        }

        public async Task<ApiResult<List<CategoryViewModel>>> List()
        {
            var query = _db.Categories.AsQueryable();
            var result = await query.Select(x => new CategoryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Other = x.Other,
                Image = x.SeoImage,
                SeoAlias = x.SeoAlias
            }).ToListAsync();
            return new ApiSuccessResult<List<CategoryViewModel>>(result);
        }
        public async Task<ApiResult<bool>> Delete(byte categoryId)
        {
            var category = await _db.Categories.FindAsync(categoryId);
            if (category == null) return new ApiErrorResult<bool>("Không tìm thấy danh mục");

            category.Status = false;
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> PermanentlyDelete(byte categoryId)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var category = await _db.Categories.FindAsync(categoryId);
                if (category == null) return new ApiErrorResult<bool>("Không tìm thấy danh mục");

                var deleteImage = await _image.DeleteImage(SetFolder(category.SeoAlias) + "/" + category.SeoAlias);
                if (deleteImage)
                    await _image.DeleteFolderImage(SetFolder(category.SeoAlias));

                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();

                await _db.Database.CommitTransactionAsync();
                return new ApiSuccessResult<bool>();
            }
            catch (Exception ex)
            {
                await _db.Database.RollbackTransactionAsync();
                return new ApiErrorResult<bool>(ex.Message);
            }
        }

        public async Task<ApiResult<bool>> Update(CategoryCreateUpdateRequest request, byte categoryId)
        {
            var alias = StringHelper.GenerateSeoAlias(request.Name);
            var categoryUpdate = await _db.Categories.FindAsync(categoryId);
            if (categoryUpdate == null) return new ApiErrorResult<bool>("Không tìm thấy danh mục");
            if (!string.IsNullOrEmpty(request.Name))
            {
                categoryUpdate.SeoAlias = alias;
                categoryUpdate.Name = request.Name;
            }
            if (!string.IsNullOrEmpty(request.Description))
                categoryUpdate.SeoDescription = request.Description;
            if (!string.IsNullOrEmpty(request.Other))
                categoryUpdate.Other = request.Other;
            if (request.Image != null)
            {
                //folder\\category\\tên danh mục\\ảnh
                var deleteOldImage = await _image.DeleteImage(SetFolder(categoryUpdate.SeoAlias) + "/" + categoryUpdate.SeoAlias);
                //Root/category/seoAlias = window
                await _image.DeleteFolderImage(SetFolder(categoryUpdate.SeoAlias));

                if (deleteOldImage)
                {
                    var folder = string.IsNullOrEmpty(alias) ? categoryUpdate.SeoAlias : alias;
                    var newImage = await _image.UploadImage(request.Image, folder, SetFolder($"{folder}"));
                    if (newImage != null)
                        categoryUpdate.SeoImage = newImage;
                }
            }
            _db.Categories.Update(categoryUpdate);
            await _db.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        private string SetFolder(string alias)
        {
            return $"categories/{alias}";
        }
    }
}
