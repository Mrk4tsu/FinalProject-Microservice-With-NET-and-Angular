using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace FN.Application.Systems.Redis
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;
        private readonly IDistributedCache _cache;
        private readonly ISubscriber _subscriber;
        public RedisService(IDatabase database, IDistributedCache cache, IConnectionMultiplexer connectionMultiplexer)
        {
            _database = database;
            _cache = cache;
            _subscriber = connectionMultiplexer.GetSubscriber();
        }
        public async Task ListPush<T>(string key, T value)
        {
            await _database.ListRightPushAsync(key, JsonSerializer.Serialize(value));
        }
        public async Task ListTrim(string key, long start, long stop)
        {
            await _database.ListTrimAsync(key, start, stop);
        }
        public async Task<List<string>> ListSetValue(string key)
        {
            var member = await _database.SetMembersAsync(key);
            return member.Select(x => x.ToString()).ToList();
        }
        public async Task<string?> GetCache(string key)
        {
            return await _cache.GetStringAsync(key);
        }
        public async Task SetCache(string key, string value, DistributedCacheEntryOptions options)
        {
            await _cache.SetStringAsync(key, value, options);
        }
        public async Task RemoveCache(string key)
        {
            await _cache.RemoveAsync(key);
        }
        public async Task AddValue(string key, string value, TimeSpan? expiry = null)
        {
            await _database.SetAddAsync(key, value);
            if (expiry.HasValue) await _database.KeyExpireAsync(key, expiry);
        }
        public async Task RemoveSetValue(string key, string value)
        {
            await _database.SetRemoveAsync(key, value);
        }
        public List<string> GetKeysByPattern(string pattern)
        {
            var keys = new List<string>();
            var endpoints = _database.Multiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _database.Multiplexer.GetServer(endpoint);
                foreach (var key in server.Keys(pattern: pattern))
                {
                    keys.Add(key.ToString());
                }
            }
            return keys;
        }
        public async Task<T?> GetValue<T>(string key)
        {
            var json = await _database.StringGetAsync(key);
            if (json.IsNullOrEmpty)
            {
                return default!;
            }
            return JsonSerializer.Deserialize<T>(json.ToString());
        }
        public async Task SetValue<T>(string key, T value, TimeSpan? expiry = null)
        {
            await _database.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);
        }
        public async Task<bool> SetContains(string key, string value)
        {
            return await _database.SetContainsAsync(key, value);
        }
        public async Task RemoveValue(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
        public async Task Publish<T>(string channel, T message)
        {
            var serializedMessage = JsonSerializer.Serialize(message);
            await _subscriber.PublishAsync(new RedisChannel(channel, RedisChannel.PatternMode.Auto), serializedMessage);
        }
        public async Task Subscribe(string channel, Func<RedisChannel, RedisValue, Task> handler)
        {
            await _subscriber.SubscribeAsync(new RedisChannel(channel, RedisChannel.PatternMode.Auto), async (redisChannel, redisValue) =>
            {
                await handler(redisChannel, redisValue);
            });
        }
        public async Task<bool> KeyExist(string key)
        {
            return await _database.KeyExistsAsync(key);
        }
    }
}
