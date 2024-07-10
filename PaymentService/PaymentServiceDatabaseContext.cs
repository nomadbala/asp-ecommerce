using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService;

public class PaymentServiceDatabaseContext(DbContextOptions<PaymentServiceDatabaseContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments { get; init; }
}   