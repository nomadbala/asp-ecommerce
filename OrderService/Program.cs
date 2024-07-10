using Microsoft.EntityFrameworkCore;
using OrderService;
using OrderService.HttpClients;
using OrderService.Repositories;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderServiceDatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();

builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

builder.Services.AddHttpClient<IUsersHttpClient, UsersHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://userservice:80");
});

builder.Services.AddHttpClient<IProductsHttpClient, ProductsHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://productservice:80/");
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