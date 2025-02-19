using AutoMapper;
using FN.Application.Helper.Images;
using FN.Application.Systems.Redis;
using FN.DataAccess;
using FN.DataAccess.Entities;
using FN.Utilities;
using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Paging;
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
            var code = StringHelper.GenerateProductCode(request.Title);
            var folder = Folder(code);

            var thumbnail = await _image.UploadImage(request.Thumbnail, code, folder);
            var newItem = new Item()
            {
                Title = request.Title,
                Description = request.Description,
                Keywords = request.Keywords,
                SeoTitle = request.SeoTitle,
                UserId = userId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                NormalizedTitle = StringHelper.NormalizeString(request.Title),
                SeoAlias = StringHelper.GenerateSeoAlias(request.SeoTitle),
                Code = code,
                Thumbnail = thumbnail ?? "",
            };

            await _db.Items.AddAsync(newItem);
            await _db.SaveChangesAsync();
            return new ApiSuccessResult<int>(newItem.Id);
        }

        public Task<ApiResult<PagedResult<ProductViewModel>>> GetProducts()
        {
            throw new NotImplementedException();
        }
        const string ROOT = "product";
        private string Folder(string code)
        {
            return $"{ROOT}/{code}";
        }
    }
}
