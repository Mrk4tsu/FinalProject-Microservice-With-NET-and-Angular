using FN.Application.Helper.Images;
using FN.DataAccess;
using FN.DataAccess.Entities;
using FN.Utilities;
using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;

namespace FN.Application.Catalog.Product.Pattern
{
    public class CreateProductFacade
    {
        private readonly AppDbContext _db;
        private readonly IImageService _image;
        const string ROOT = "product";
        public CreateProductFacade(AppDbContext db, IImageService image)
        {
            _db = db;
            _image = image;
        }
        public async Task<ApiResult<int>> Create(CreateProductRequest request, int userId)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var item = await CreateItem(request, userId);

                await transaction.CommitAsync();
                return new ApiSuccessResult<int>(item.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiErrorResult<int>(ex.ToString());
            }
        }
        private async Task<Item> CreateItem(CreateProductRequest request, int userId)
        {
            var code = StringHelper.GenerateProductCode(request.Title);
            var folder = Folder(code);
            DateTime timeNow = new TimeHelper.Builder()
                .SetTimestamp(DateTime.UtcNow)
                .SetTimeZone("SE Asia Standard Time")
                .SetRemoveTick(true).Build();
            var thumbnail = await _image.UploadImage(request.Thumbnail, code, folder);
            var newItem = new Item()
            {
                Title = request.Title,
                Description = request.Description,
                Keywords = request.Keywords,
                SeoTitle = request.SeoTitle,
                UserId = userId,
                CreatedDate = timeNow,
                ModifiedDate = timeNow,
                NormalizedTitle = StringHelper.NormalizeString(request.Title),
                SeoAlias = StringHelper.GenerateSeoAlias(request.SeoTitle),
                Code = code,
                Thumbnail = thumbnail ?? "",
            };

            await _db.Items.AddAsync(newItem);
            await _db.SaveChangesAsync();

            return newItem;
        }
        private string Folder(string code)
        {
            return $"{ROOT}/{code}";
        }
    }
}
