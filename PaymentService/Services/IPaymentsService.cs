using PaymentService.Contracts;
using PaymentService.Models;

namespace PaymentService.Services;

public interface IPaymentsService
{
    Task<IEnumerable<Payment>> GetAllAsync();

    Task<Payment> CreateAsync(CreatePaymentContract contract);

    Task<Payment> GetByIdAsync(Guid id);

    Task<Payment> UpdateAsync(Guid id, UpdatePaymentContract contract);

    Task DeleteByIdAsync(Guid id);

    Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId);

    Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId);

    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
}