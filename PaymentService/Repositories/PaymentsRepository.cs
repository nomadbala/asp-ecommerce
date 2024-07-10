using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Repositories;

public class PaymentsRepository : IPaymentsRepository
{
    private readonly PaymentServiceDatabaseContext _context;

    public PaymentsRepository(PaymentServiceDatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public Task<Payment> CreateAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Payment> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Payment> UpdateAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Payment>> GetByUserIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
    {
        throw new NotImplementedException();
    }
}