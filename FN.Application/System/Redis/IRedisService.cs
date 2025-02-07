using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace FN.Application.System.Redis
{
    public interface IRedisService
    {
        Task<bool> KeyExist(string key);

        // Các phương thức thao tác trực tiếp với Redis thông qua IDatabase
        Task SetValue<T>(string key, T value, TimeSpan? expiry = null);
        Task<T?> GetValue<T>(string key);

        // Các phương thức thao tác với distributed cache qua IDistributedCache
        Task SetCache(string key, string value, DistributedCacheEntryOptions options);
        Task<string?> GetCache(string key);

        Task Publish<T>(string channel, T message);
        Task Subscribe(string channel, Func<RedisChannel, RedisValue, Task> handler);
    }
}
