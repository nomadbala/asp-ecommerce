using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService;

public class ProductServiceDatabaseContext(DbContextOptions<ProductServiceDatabaseContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; init; }
}