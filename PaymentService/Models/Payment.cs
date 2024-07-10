using OrderService.Models;
using UserService.Models;

namespace PaymentService.Models;

public enum PaymentStatus
{
    Successful,
    Unsuccessful
}

public class Payment
{
    public Payment()
    {
    }

    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    
    public Guid OrderId { get; set; }
    
    public Order Order { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateTime PaymentDate { get; set; }
    
    public PaymentStatus Status { get; set; }
}