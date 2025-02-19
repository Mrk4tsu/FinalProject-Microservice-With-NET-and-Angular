using FN.Application.Helper.Images;
using FN.Application.Systems.Redis;
using FN.DataAccess;
using FN.Utilities;

namespace FN.Application.Catalog.Product.Pattern
{
    public class BaseService
    {
        protected const string ROOT = "product";
        protected readonly IRedisService _dbRedis;
        protected readonly IImageService _image;
        protected readonly AppDbContext _db;
        public BaseService(AppDbContext db, IRedisService dbRedis, IImageService image)
        {
            _db = db;
            _dbRedis = dbRedis;
            _image = image;
        }
        protected async Task RemoveOldCache()
        {
            await _dbRedis.RemoveValue(SystemConstant.CACHE_PRODUCT);
        }
        protected string Folder(string code)
        {
            return $"{ROOT}/{code}";
        }
    }
}
