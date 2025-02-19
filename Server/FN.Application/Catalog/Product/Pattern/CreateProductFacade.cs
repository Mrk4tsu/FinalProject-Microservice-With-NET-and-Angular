using FN.Application.Helper.Images;
using FN.DataAccess;
using FN.DataAccess.Entities;
using FN.DataAccess.Enums;
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
                var productDetail = await CreateProductDetail(request, item.Id);
                await CreatePrice(request, productDetail.Id);
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
        private async Task<ProductDetail> CreateProductDetail(CreateProductDetailRequest request, int itemId)
        {
            var productDetail = new ProductDetail()
            {
                Detail = request.Detail,
                Version = request.Version,
                Note = request.Note,
                ItemId = itemId,
                CategoryId = request.CategoryId,
                Status = request.Status
            };
            await _db.ProductDetails.AddAsync(productDetail);
            await _db.SaveChangesAsync();
            return productDetail;
        }
        private async Task CreatePrice(CreatePriceRequest request, int productDetailId)
        {
            var price = new ProductPrice()
            {
                Price = request.Price,
                ProductDetailId = productDetailId,
                CreatedDate = DateTime.UtcNow,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.MaxValue,
                PriceType = PriceType.BASE
            };

            await _db.ProductPrices.AddAsync(price);
            await _db.SaveChangesAsync();
        }
        private string Folder(string code)
        {
            return $"{ROOT}/{code}";
        }
    }
}
