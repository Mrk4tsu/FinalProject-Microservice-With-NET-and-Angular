using AutoMapper;
using FN.Application.Catalog.Product.Pattern;
using FN.Application.Systems.Redis;
using FN.DataAccess;
using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Paging;
using Microsoft.EntityFrameworkCore;

namespace FN.Application.Catalog.Product
{
    public class ProductPublicService : IProductPublicService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IRedisService _dbRedis;
        public ProductPublicService(AppDbContext db, IMapper mapper, IRedisService redis)
        {
            _mapper = mapper;
            _db = db;
            _dbRedis = redis;
        }

        public async Task<ApiResult<ProductDetailViewModel>> GetProduct(int itemId)
        {
            var product = await _db.ProductDetails
                .Include(x => x.Item)
                .ThenInclude(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.ProductPrices)
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => x.ItemId == itemId);
            if (product == null) return new ApiErrorResult<ProductDetailViewModel>("Không tìm thấy sản phẩm");
            var detailVM = new ProductDetailViewModel
            {
                Id = product.Id,
                CategoryIcon = product.Category.SeoImage,
                Title = product.Item.Title,
                Detail = product.Detail,
                LikeCount = product.LikeCount,
                DisLikeCount = product.DislikeCount,
                DownloadCount = product.DownloadCount,
                Version = product.Version,
                Note = product.Note,
                CategoryName = product.Category.Name,
                SeoAlias = product.Item.SeoAlias,
                TimeCreates = product.Item.CreatedDate,
                TimeUpdates = product.Item.ModifiedDate,
                CategorySeoAlias = product.Category.SeoAlias,
                Description = product.Item.Description,
                Price = product.ProductPrices.Count > 0 ? product.ProductPrices[0].Price : 0,
                Thumbnail = product.Item.Thumbnail,
                Username = product.Item.User.UserName!,
                ViewCount = product.Item.ViewCount,
            };
            return new ApiSuccessResult<ProductDetailViewModel>(detailVM);
        }

        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetProducts(ProductPagingRequest request)
        {
            var facade = new GetProductFacade(_db, _dbRedis!, null!);
            return await facade.GetProducts(request, false, false, null);
        }
    }
}
