using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

builder.Configuration.AddJsonFile("apigateway.appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"apigateway.appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");

app.MapControllers();

app.UseRouting();

await app.UseOcelot();

app.Run();