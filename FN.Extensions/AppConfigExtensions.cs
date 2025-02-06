using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FN.Extensions
{
    public static class AppConfigExtensions
    {
        public static IApplicationBuilder ConfigureCORS(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
            return app;
        }
        public static IServiceCollection AddAppConfig(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }
    }
}
