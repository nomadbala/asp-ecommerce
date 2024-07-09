using OrderService.Contracts;
using OrderService.Models;

namespace OrderService.Services;

public interface IOrdersService
{
    Task<IEnumerable<Order>> GetAllAsync();

    Task<Order> CreateAsync(CreateOrderContract contract);

    Task<Order> GetByIdAsync(Guid id);

    Task<Order> UpdateAsync(Guid id, UpdateOrderContract contract);

    Task DeleteByIdAsync(Guid id);

    Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId);

    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
}