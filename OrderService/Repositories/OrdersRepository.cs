using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using OrderService.Contracts;
using OrderService.Models;
using ProductService.Services;
using UserService.Services;

namespace OrderService.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrderServiceDatabaseContext _context;

    private readonly IUsersService _usersService;

    private readonly IProductsService _productsService;

    public OrdersRepository(OrderServiceDatabaseContext context, IUsersService usersService, IProductsService productsService)
    {
        _context = context;
        _usersService = usersService;
        _productsService = productsService;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order> CreateAsync(CreateOrderContract contract)
    {
        var customer = await _usersService.GetByIdAsync(contract.CustomerId);

        if (customer == null)
            throw new ElementNotFoundException($"Customer with id {contract.CustomerId} not found");

        var products = (await _productsService.GetByIdsAsync(contract.ProductIds)).ToList();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            Customer = customer,
            ProductIds = contract.ProductIds,
            Products = products.ToList(),
            OrderDate = DateTime.UtcNow,
            Status = contract.Status
        };

        await _context.AddAsync(order);
        await _context.SaveChangesAsync();

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

        if (order == null)
            throw new ElementNotFoundException($"Order with id {id} not found");

        var products = (await _productsService.GetByIdsAsync(contract.ProductIds)).ToList();

        order.ProductIds = products.Select(p => p.Id).ToList();
        order.Products = products;
        order.Status = contract.Status;

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var order = await GetByIdAsync(id);

        if (order == null)
            throw new ElementNotFoundException($"Order with id {id} not found");

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