using OrderService.Models;

namespace PaymentService.HttpClients;

public interface IOrdersHttpClient
{
    Task<Order> GetByIdAsync(Guid id);
}