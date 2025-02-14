using FN.Application.Helper.Mail;
using FN.Utilities;
using FN.ViewModel.Helper;
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
        public static IServiceCollection AddSmtpConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MailSetting>(config.GetSection(SystemConstant.SMTP_SETTINGS));
            services.AddSingleton<IMailService, MailService>();
            return services;
        }
    }
}
