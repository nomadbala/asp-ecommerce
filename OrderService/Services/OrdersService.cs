using OrderService.Contracts;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderService.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _repository;

    public OrdersService(IOrdersRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Order> CreateAsync(CreateOrderContract contract)
    {
        if (contract == null)
            throw new BadHttpRequestException(nameof(contract));

        if (!contract.ProductIds.Any())
            throw new BadHttpRequestException($"ProductIds must contain at least one property. {nameof(contract.ProductIds)}");
        
        return await _repository.CreateAsync(contract);
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadHttpRequestException($"Invalid ID. {nameof(id)}");
        
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Order> UpdateAsync(Guid id, UpdateOrderContract contract)
    {
        if (id == Guid.Empty)
            throw new BadHttpRequestException($"Invalid ID. {nameof(id)}");

        if (!contract.ProductIds.Any())
            throw new BadHttpRequestException($"ProductIds must contain at least one property. {nameof(contract.ProductIds)}");
        
        return await _repository.UpdateAsync(id, contract);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadHttpRequestException($"Invalid ID. {nameof(id)}");
        
        await _repository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new BadHttpRequestException($"Invalid ID. {nameof(customerId)}");
        
        return await _repository.GetByCustomerIdAsync(customerId);
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), status))
            throw new BadHttpRequestException($"Invalid order status. {nameof(status)}");
        
        return await _repository.GetByStatusAsync(status);
    }
}