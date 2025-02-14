using FN.Application.Systems.Redis;
using FN.Utilities;
using FN.ViewModel.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StackExchange.Redis;

namespace FN.Extensions
{
    public static class NoSQLExtensions
    {
        public static IServiceCollection InjectMongoDb(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MongoDBSettings>(config.GetSection(SystemConstant.MONGODB_SETTING));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
                return client.GetDatabase(settings.DatabaseName);
            });
            return services;
        }
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
    }
}
