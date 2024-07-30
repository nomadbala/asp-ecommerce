using PaymentService.Contracts;
using PaymentService.Models;

namespace PaymentService.Repositories;

public interface IPaymentsRepository
{
    Task<IEnumerable<Payment>> GetAllAsync();

    Task<Payment> CreateAsync(CreatePaymentContract contract);

    Task<Payment> Save(Payment payment);

    Task<Payment> GetByIdAsync(Guid id);

    Task<Payment> UpdateAsync(Guid id, UpdatePaymentContract contract);

    Task DeleteByIdAsync(Guid id);

    Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId);

    Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId);

    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
}