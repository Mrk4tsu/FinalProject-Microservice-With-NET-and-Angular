using FN.EmailService;
using FN.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<MailSubscriber>();

builder.Services
    .InjectRedis(builder.Configuration)
    .AddSmtpConfig(builder.Configuration);

builder.Services.AddScoped<MailSubscriber>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
