using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using PaymentService.Contracts;
using PaymentService.HttpClients;
using PaymentService.Models;

namespace PaymentService.Repositories;

public class PaymentsRepository : IPaymentsRepository
{
    private readonly PaymentServiceDatabaseContext _context;

    private readonly IUsersHttpClient _usersHttpClient;

    private readonly IOrdersHttpClient _ordersHttpClient;

    public PaymentsRepository(PaymentServiceDatabaseContext context, IUsersHttpClient usersHttpClient, IOrdersHttpClient ordersHttpClient)
    {
        _context = context;
        _usersHttpClient = usersHttpClient;
        _ordersHttpClient = ordersHttpClient;
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task<Payment> CreateAsync(CreatePaymentContract contract)
    {
        var user = await _usersHttpClient.GetByIdAsync(contract.UserId);

        if (user == null)
            throw new ElementNotFoundException($"User with id {contract.UserId} not found");

        var order = await _ordersHttpClient.GetByIdAsync(contract.OrderId);

        if (order == null)
            throw new ElementNotFoundException($"Order with id {contract.OrderId} not found");
    
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            OrderId = order.Id,
            Amount = contract.Amount,
            PaymentDate = DateTime.UtcNow,
            Status = contract.Status
        };

        await _context.AddAsync(payment);
        await _context.SaveChangesAsync();

        payment.User = user;
        payment.Order = order;

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