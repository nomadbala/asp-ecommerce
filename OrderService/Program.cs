using Microsoft.EntityFrameworkCore;
using OrderService;
using OrderService.HttpClients;
using OrderService.Repositories;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("orderservice.appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"orderservice.appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<OrderServiceDatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();

builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

builder.Services.AddHttpClient<IUsersHttpClient, UsersHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://userservice:8080");
});

builder.Services.AddHttpClient<IProductsHttpClient, ProductsHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://productservice:8080");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();