using FN.Application.Helper.Images;
using FN.Application.Systems.Redis;
using FN.DataAccess;
using FN.Utilities;
using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Paging;
using Microsoft.EntityFrameworkCore;

namespace FN.Application.Catalog.Product.Pattern
{
    public class GetProductFacade : BaseService
    {
        public GetProductFacade(AppDbContext db, IRedisService dbRedis, IImageService image) : base(db, dbRedis, image)
        {
        }

        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetProductsOptimized(ProductPagingRequest request, bool isMe, int? currentUserId)
        {
            const string cacheKey = SystemConstant.CACHE_PRODUCT;
            List<ProductViewModel>? cachedData = null;
            bool useCache = await _dbRedis.KeyExist(cacheKey);

            if (useCache)
            {
                cachedData = await _dbRedis.GetValue<List<ProductViewModel>>(cacheKey);
            }

            PagedResult<ProductViewModel> result;

            if (useCache && cachedData != null)
            {
                var filteredData = ApplyMemoryFilters(cachedData, request, isMe, currentUserId);
                result = CreatePagedResult(filteredData, request);
            }
            else
            {
                var query = BuildBaseQuery();
                var filteredQuery = ApplyDatabaseFilters(query, request, isMe, currentUserId);
                result = await ExecuteDatabasePaging(filteredQuery, request);

                await CacheBaseData(query, cacheKey);
            }

            return new ApiSuccessResult<PagedResult<ProductViewModel>>(result);
        }
        private IQueryable<ProductViewModel> BuildBaseQuery()
        {
            return _db.Items
                .AsNoTracking()
                .Where(i => !i.IsDeleted)
                .Join(_db.ProductDetails,
                    item => item.Id,
                    detail => detail.ItemId,
                    (item, detail) => new { item, detail })
                .Join(_db.Categories,
                    combined => combined.detail.CategoryId,
                    category => category.Id,
                    (combined, category) => new { combined.item, combined.detail, category })
                .Join(_db.ProductPrices,
                    combined => combined.detail.Id,
                    price => price.ProductDetailId,
                    (combined, price) => new { combined.item, combined.detail, combined.category, price })
                .Join(_db.Users,
                    combined => combined.item.UserId,
                    user => user.Id,
                    (combined, user) => new ProductViewModel
                    {
                        Id = combined.item.Id,
                        Title = combined.item.Title,
                        NormalizeTitle = combined.item.NormalizedTitle,
                        CategorySeoAlias = combined.category.SeoAlias,
                        SeoAlias = combined.item.SeoAlias,
                        CategoryIcon = combined.category.SeoImage,
                        DownloadCount = combined.detail.DownloadCount,
                        TimeCreates = combined.item.CreatedDate,
                        TimeUpdates = combined.item.ModifiedDate,
                        Thumbnail = combined.item.Thumbnail,
                        Username = user.FullName,
                        UserId = combined.item.UserId,
                        Version = combined.detail.Version,
                        Price = combined.price.Price
                    });
        }
        private List<ProductViewModel> ApplyMemoryFilters(List<ProductViewModel> data, ProductPagingRequest request, bool isMe, int? currentUserId)
        {
            var query = data.AsQueryable();
            if (isMe && currentUserId.HasValue) query = query.Where(x => x.UserId == currentUserId.Value);
            if (!string.IsNullOrEmpty(request.CategorySeoAlias)) query = query.Where(x => x.CategorySeoAlias == request.CategorySeoAlias);
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                var kw = request.KeyWord.Trim().ToUpperInvariant();
                query = query.Where(x =>
                    x.NormalizeTitle.Contains(kw) ||
                    x.Title.Contains(request.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                    x.Username.Contains(request.KeyWord, StringComparison.OrdinalIgnoreCase));
            }
            return query
               .OrderByDescending(x => x.TimeCreates)
               .ToList();
        }

        private IQueryable<ProductViewModel> ApplyDatabaseFilters(IQueryable<ProductViewModel> query, ProductPagingRequest request, bool isMe, int? currentUserId)
        {
            if (isMe && currentUserId.HasValue)
            {
                query = query.Where(x => x.UserId == currentUserId.Value);
            }
            if (!string.IsNullOrEmpty(request.CategorySeoAlias))
            {
                query = query.Where(x => x.CategorySeoAlias == request.CategorySeoAlias);
            }

            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                var kw = request.KeyWord.Trim().ToUpperInvariant();
                query = query.Where(x =>
                    x.NormalizeTitle.Contains(kw) ||
                    EF.Functions.Like(x.Title, $"%{request.KeyWord}%") ||
                    EF.Functions.Like(x.Username, $"%{request.KeyWord}%"));
            }

            return query;
        }
        private async Task<PagedResult<ProductViewModel>> ExecuteDatabasePaging(IQueryable<ProductViewModel> query, ProductPagingRequest request)
        {
            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.TimeCreates)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<ProductViewModel>
            {
                TotalRecords = total,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = items
            };
        }
        private PagedResult<ProductViewModel> CreatePagedResult(List<ProductViewModel> data, ProductPagingRequest request)
        {
            return new PagedResult<ProductViewModel>
            {
                TotalRecords = data.Count,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList()
            };
        }
        private async Task CacheBaseData(IQueryable<ProductViewModel> query, string cacheKey)
        {
            var cacheData = await query
                .OrderByDescending(x => x.TimeCreates)
                .ToListAsync();

            await _dbRedis.SetValue(cacheKey, cacheData);
        }
    }
}
