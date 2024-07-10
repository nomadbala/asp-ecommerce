using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using OrderService.Services;
using PaymentService.Contracts;
using PaymentService.Models;
using UserService.Services;

namespace PaymentService.Repositories;

public class PaymentsRepository : IPaymentsRepository
{
    private readonly PaymentServiceDatabaseContext _context;

    private readonly IUsersService _usersService;

    private readonly IOrdersService _ordersService;

    public PaymentsRepository(PaymentServiceDatabaseContext context, IUsersService usersService, IOrdersService ordersService)
    {
        _context = context;
        _usersService = usersService;
        _ordersService = ordersService;
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task<Payment> CreateAsync(CreatePaymentContract contract)
    {
        var user = await _usersService.GetByIdAsync(contract.UserId);

        if (user == null)
            throw new ElementNotFoundException($"User with id {contract.UserId} not found");

        var order = await _ordersService.GetByIdAsync(contract.OrderId);

        if (order == null)
            throw new ElementNotFoundException($"Order with id {contract.OrderId} not found");
    
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            OrderId = order.Id,
            Order = order,
            Amount = contract.Amount,
            PaymentDate = DateTime.UtcNow,
            Status = contract.Status
        };

        await _context.AddAsync(payment);
        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task<Payment> GetByIdAsync(Guid id)
    {
        var payment = await _context.Payments.FindAsync(id);

        return payment ?? throw new ElementNotFoundException($"Payment with id {id} not found");
    }

    public async Task<Payment> UpdateAsync(Guid id, UpdatePaymentContract contract)
    {
        var payment = await GetByIdAsync(id);

        payment.Amount = contract.Amount;
        payment.Status = contract.Status;

        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var payment = await GetByIdAsync(id);

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Payments
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.Payments
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
    {
        return await _context.Payments
            .Where(p => p.Status == status)
            .ToListAsync();
    }
}