using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddControllers();

builder.Services.AddOcelot();

builder.Configuration.AddJsonFile("apigateway.appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("apigateway.appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.UseRouting();

app.UseOcelot().Wait();

app.Run();