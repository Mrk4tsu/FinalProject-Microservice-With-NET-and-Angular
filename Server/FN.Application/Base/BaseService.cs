using FN.Application.Systems.Redis;
using Microsoft.Extensions.Configuration;

namespace FN.Application.Base
{
    public class BaseService
    {
        protected const string Folder = "product";
        protected readonly IRedisService _dbRedis;
        protected readonly IConfiguration _configuration;
        public BaseService(IRedisService dbRedis, IConfiguration configuration)
        {
            _dbRedis = dbRedis;
            _configuration = configuration;
        }
        protected string SetFolder(string code)
        {
            return $"{Folder}/{code}";
        }
        protected DateTime Now()
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

            return timeNow;
        }
    }
}
