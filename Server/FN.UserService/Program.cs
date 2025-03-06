using FN.Application.Systems.Token;
using FN.Application.Systems.User;
using FN.Extensions;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerExplorer()
    .InjectDbContext(builder.Configuration)
    .InjectRedis(builder.Configuration)
    .InjectMongoDb(builder.Configuration)
    .AddIdentityHandlersAndStores()
    .AddIdentityAuth(builder.Configuration)
    .ConfigureIdentityOptions()
    .AddImageConfig(builder.Configuration);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();


var app = builder.Build();

// Thêm middleware để log số kết nối
app.Use(async (context, next) =>
{
    var connection = context.RequestServices.GetService<IDbConnection>();
    Console.WriteLine($"Connection State: {connection!.State}");

    await next();
});

app.ConfigureSwaggerExplorer()
    .ConfigureCORS(builder.Configuration)
    .ConfigureAppExplorer()
    .AddIdentityAuthMiddlewares();

app.MapControllers();

app.Run();
