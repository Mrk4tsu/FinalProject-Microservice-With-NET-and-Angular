using AutoMapper;
using FN.Application.Catalog.Product.Pattern;
using FN.Application.Helper.Images;
using FN.Application.Systems.Redis;
using FN.DataAccess;
using FN.DataAccess.Entities;
using FN.Utilities;
using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Paging;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;

namespace FN.Application.Catalog.Product
{
    public class ProductManageService : IProductManageService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IImageService _image;
        private readonly IRedisService _dbRedis;
        private readonly IConfiguration _config;
        public ProductManageService(AppDbContext db,
            IConfiguration configuration,
            IRedisService redisService, IMapper mapper, IImageService image)
        {
            _dbRedis = redisService;
            _db = db;
            _mapper = mapper;
            _image = image;
            _config = configuration;
        }

        public async Task<ApiResult<int>> Create(CreateProductRequest request, int userId)
        {
            var facade = new CreateProductFacade(_db, _dbRedis, _image);
            return await facade.Create(request, userId);
        }

        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetProducts(ProductPagingRequest request, int userId)
        {
            var facade = new GetProductFacade(_db, _dbRedis, _image);
            return await facade.GetProductsOptimized(request, true, userId);
        }

    }
}
