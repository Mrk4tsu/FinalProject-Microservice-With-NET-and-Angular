using FN.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var evn = builder.Environment;
Console.WriteLine(evn.EnvironmentName);
builder.Configuration.AddJsonFile($"ocelot.{evn.EnvironmentName}.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.ConfigureCORS(builder.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.UseOcelot();

app.Run();
