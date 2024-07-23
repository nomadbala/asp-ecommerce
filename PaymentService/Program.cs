using Microsoft.EntityFrameworkCore;
using PaymentService;
using PaymentService.HttpClients;
using PaymentService.Repositories;
using PaymentService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("paymentservice.appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"paymentservice.appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<PaymentServiceDatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddHttpClient<IEpayService, EpayService>();

builder.Services.AddScoped<IPaymentsRepository, PaymentsRepository>();
builder.Services.AddScoped<IPaymentsService, PaymentsService>();

builder.Services.AddHttpClient<IUsersHttpClient, UsersHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://userservice:8080");
});

builder.Services.AddHttpClient<IOrdersHttpClient, OrdersHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://orderservice:8080");
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();