using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService;

public class OrderServiceDatabaseContext(DbContextOptions<OrderServiceDatabaseContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
}