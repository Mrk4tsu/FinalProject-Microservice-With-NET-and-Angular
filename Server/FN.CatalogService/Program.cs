using FN.Application.Catalog.Categories;
using FN.Application.Catalog.Product;
using FN.Extensions;

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

builder.Services.AddScoped<IProductPublicService, ProductPublicService>();
builder.Services.AddScoped<IProductManageService, ProductManageService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
var app = builder.Build();

app.ConfigureSwaggerExplorer()
    .ConfigureCORS(builder.Configuration)
    .ConfigureAppExplorer()
    .AddIdentityAuthMiddlewares();

app.MapControllers();

app.Run();
