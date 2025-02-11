using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace FN.Application.Systems.Redis
{
    public interface IRedisService
    {
        Task<bool> KeyExist(string key);

        // Các phương thức thao tác trực tiếp với Redis thông qua IDatabase
        Task ListPush<T>(string key, T value);
        Task ListTrim(string key, long start, long stop);
        Task<List<string>> ListSetValue(string key);
        Task AddValue(string key, string value, TimeSpan? expiry = null);
        Task SetValue<T>(string key, T value, TimeSpan? expiry = null);
        Task<T?> GetValue<T>(string key);
        Task<bool> SetContains(string key, string value);
        Task RemoveSetValue(string key, string value);
        Task RemoveValue(string key);

        // Các phương thức thao tác với distributed cache qua IDistributedCache
        Task SetCache(string key, string value, DistributedCacheEntryOptions options);
        Task<string?> GetCache(string key);
        Task RemoveCache(string key);

        Task Publish<T>(string channel, T message);
        Task Subscribe(string channel, Func<RedisChannel, RedisValue, Task> handler);
    }
}
