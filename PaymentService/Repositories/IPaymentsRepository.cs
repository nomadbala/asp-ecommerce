using PaymentService.Models;

namespace PaymentService.Repositories;

public interface IPaymentsRepository
{
    Task<IEnumerable<Payment>> GetAllAsync();

    Task<Payment> CreateAsync();

    Task<Payment> GetByIdAsync(Guid id);

    Task<Payment> UpdateAsync(Guid id);

    Task DeleteByIdAsync(Guid id);

    Task<IEnumerable<Payment>> GetByUserIdAsync(Guid id);

    Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid id);

    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
}