using AutoMapper;
using FN.Application.Catalog.Product.Pattern;
using FN.Application.Systems.Redis;
using FN.DataAccess;
using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Paging;

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
        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetProducts(ProductPagingRequest request)
        {
            var facade = new GetProductFacade(_db, _dbRedis!, null!);
            return await facade.GetProductsOptimized(request, false, null);
        }
    }
}
