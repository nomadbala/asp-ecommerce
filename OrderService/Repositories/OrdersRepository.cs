using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using OrderService.Contracts;
using OrderService.HttpClients;
using OrderService.Models;

namespace OrderService.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrderServiceDatabaseContext _context;

    private readonly IUsersHttpClient _usersHttpClient;

    private readonly IProductsHttpClient _productsHttpClient;

    public OrdersRepository(OrderServiceDatabaseContext context, IUsersHttpClient usersHttpClient, IProductsHttpClient productsHttpClient)
    {
        _context = context;
        _usersHttpClient = usersHttpClient;
        _productsHttpClient = productsHttpClient;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order> CreateAsync(CreateOrderContract contract)
    {
        var customer = await _usersHttpClient.GetByIdAsync(contract.CustomerId);

        var products = (await _productsHttpClient.GetByIdsAsync(contract.ProductIds)).ToList();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            ProductIds = contract.ProductIds,
            OrderDate = DateTime.UtcNow,
            Status = contract.Status
        };

        await _context.AddAsync(order);
        await _context.SaveChangesAsync();

        order.Customer = customer;
        order.Products = products.ToList();

        return order;
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);

        return order ?? throw new ElementNotFoundException($"Order with id {id} not found");
    }

    public async Task<Order> UpdateAsync(Guid id, UpdateOrderContract contract)
    {
        var order = await GetByIdAsync(id);

        var products = (await _productsHttpClient.GetByIdsAsync(contract.ProductIds)).ToList();

        order.ProductIds = products.Select(p => p.Id).ToList();
        order.Products = products;
        order.Status = contract.Status;

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var order = await GetByIdAsync(id);

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
    {
        return await _context.Orders
            .Where(o => o.Status == status)
            .ToListAsync();
    }
}