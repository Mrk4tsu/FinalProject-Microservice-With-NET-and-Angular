using FN.Application.Helper.Images;
using FN.Application.Systems.Redis;
using FN.DataAccess;
using FN.DataAccess.Entities;
using FN.Utilities;
using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;

namespace FN.Application.Catalog.Product.Pattern
{
    public class UpdateProductFacade : BaseService
    {
        public UpdateProductFacade(AppDbContext db, IRedisService dbRedis, IImageService image) : base(db, dbRedis, image)
        {
        }
        public async Task<ApiResult<bool>> Update(ItemUpdateRequest request, int itemId, int userId)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var item = await UpdateItem(request, itemId, userId);
                if (item == null) return new ApiErrorResult<bool>("Không sở hữu sản phẩm này không thể chỉnh sửa");
                var productDetail =await UpdateProductDetail(request, itemId);
                if (productDetail == null) return new ApiErrorResult<bool>("Không tìm thấy chi tiết sản phẩm");
                await RemoveOldCache();
                return new ApiSuccessResult<bool>();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new ApiErrorResult<bool>(e.Message);
            }
        }
        private async Task<Item> UpdateItem(ItemUpdateRequest request, int itemId, int userId)
        {
            var code = StringHelper.GenerateProductCode(request.Title);
            var item = await _db.Items.FindAsync(itemId);
            if (item == null) return null!;
            if (userId != item.UserId) return null!;

            item.ModifiedDate = Now();

            if (!string.IsNullOrEmpty(request.Title))
            {
                item.Title = request.Title;
                item.Code = code;
                item.SeoAlias = StringHelper.GenerateSeoAlias(request.Title);
                item.SeoTitle = request.Title;
                item.NormalizedTitle = StringHelper.NormalizeString(request.Title);
            }
            if (!string.IsNullOrEmpty(request.Description))
                item.Description = request.Description;
            if (!string.IsNullOrEmpty(request.Keywords))
                item.Keywords = request.Keywords;
            if (request.Thumbnail != null)
            {
                var newImageName = string.IsNullOrEmpty(request.Title) ? item.Code : code;
                var newThumnail = await _image.UploadImage(request.Thumbnail, newImageName, Folder(newImageName));
                if (!string.IsNullOrEmpty(newThumnail)) item.Thumbnail = newThumnail;
            }

            _db.Items.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }
        private async Task<ProductDetail> UpdateProductDetail(ProductDeatilUpdateRequest request, int itemId)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedTags.Add("code");
            var productDetail = await _db.ProductDetails.FirstOrDefaultAsync(x => x.ItemId == itemId);
            if (productDetail == null)
                return null!;
            productDetail.Detail = sanitizer.Sanitize(request.Detail);
            productDetail.Version = request.Version;
            productDetail.Note = request.Note;
            productDetail.CategoryId = request.CategoryId;
            productDetail.Status = request.Status;
            _db.ProductDetails.Update(productDetail);
            await _db.SaveChangesAsync();
            return productDetail;
        }
    }
}
