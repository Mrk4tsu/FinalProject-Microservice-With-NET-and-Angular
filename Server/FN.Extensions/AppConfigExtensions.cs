using CloudinaryDotNet;
using FN.Application.Helper.Images;
using FN.Application.Helper.Mail;
using FN.Utilities;
using FN.ViewModel.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Middleware;

namespace FN.Extensions
{
    public static class AppConfigExtensions
    {
        public static IApplicationBuilder ConfigureCORS(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200","https://mrkatsu.io.vn")
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
        public static IServiceCollection AddImageConfig(this IServiceCollection services, IConfiguration config)
        {
            var cloudinarySettings = config.GetSection(SystemConstant.CLOUDINARY_SETTINGS).Get<CloudinarySettings>();
            services.AddSingleton(new Cloudinary(new Account(
                cloudinarySettings!.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            )));

            services.AddSingleton<IImageService, ImageService>();
            return services;
        }
        public static IApplicationBuilder ConfigureAppExplorer(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            return app;
        }
        public static IApplicationBuilder ConfigureOcelot(this IApplicationBuilder app, IConfiguration config)
        {
            var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
            Console.WriteLine(env.EnvironmentName);
            var ocelotConfigFile = $"ocelot.{env.EnvironmentName}.json";

            if (!File.Exists(ocelotConfigFile))
            {
                throw new FileNotFoundException($"Ocelot configuration file '{ocelotConfigFile}' not found.");
            }
            var ocelotConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ocelotConfigFile, optional: false, reloadOnChange: true)
                .Build();

            app.UseOcelot().Wait();

            return app;
        }

    }
}
