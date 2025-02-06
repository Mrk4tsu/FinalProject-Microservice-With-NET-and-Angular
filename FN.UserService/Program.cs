using FN.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerExplorer()
    .InjectDbContext(builder.Configuration);

var app = builder.Build();

app.ConfigureSwaggerExplorer();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
