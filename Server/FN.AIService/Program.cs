using FN.Application.Catalog.Blogs;
using FN.Extensions;
using GeminiAIDev.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IImageSearchService, ImageSearchService>();

string apiKey = "AIzaSyBD-HnVjKZHWnc-7_PyezzM4OGYs2FN77Q";
//builder.Services.AddSingleton(new GeminiApiClient(apiKey));
builder.Services.AddSingleton<GameTitleExtractor>();
builder.Services.AddScoped<GeminiApiClient>(provide => new GeminiApiClient(
    provide.GetRequiredService<GameTitleExtractor>(),
    provide.GetRequiredService<IImageSearchService>(),
    apiKey
    ));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerExplorer();

var app = builder.Build();

app.UseCors(option => option
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Dispostion"));

app.ConfigureSwaggerExplorer();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
