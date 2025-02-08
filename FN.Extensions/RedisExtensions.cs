using FN.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using FN.Application.System.Redis;

namespace FN.Extensions
{
    public static class RedisExtensions
    {
        private static Lazy<ConnectionMultiplexer> _lazyConnection = null!;
        public static IServiceCollection InjectRedis(this IServiceCollection services, IConfiguration config)
        {
            var connectionStringRedis = config.GetConnectionString(SystemConstant.REDIS_CONNECTION_STRING);
            if (string.IsNullOrEmpty(connectionStringRedis))
                throw new ArgumentNullException(nameof(connectionStringRedis), "Redis connection string is missing.");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionStringRedis;
            });
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(connectionStringRedis);
            });
            services.AddSingleton(sp =>
            {
                var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return multiplexer.GetDatabase();
            });

            services.AddSingleton<IRedisService, RedisService>();
            return services;
        }
        //public static IServiceCollection InjectRedis(this IServiceCollection services, IConfiguration config)
        //{
        //    var connectionStringRedis = config.GetConnectionString(SystemConstant.REDIS_CONNECTION_STRING);
        //    if (string.IsNullOrEmpty(connectionStringRedis))
        //        throw new ArgumentNullException(nameof(connectionStringRedis), "Redis connection string is missing.");
        //    _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionStringRedis));

        //    services.AddStackExchangeRedisCache(options =>
        //    {
        //        options.Configuration = connectionStringRedis;
        //    });
        //    services.AddSingleton<IConnectionMultiplexer>(sp => _lazyConnection.Value);
        //    services.AddSingleton<IDatabase>(sp => _lazyConnection.Value.GetDatabase());
        //    services.AddSingleton<IRedisService, RedisService>();

        //    return services;
        //}

    }
}
