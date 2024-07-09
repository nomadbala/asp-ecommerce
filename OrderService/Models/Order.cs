using OrderService.Contracts;
using ProductService.Models;
using UserService.Models;

namespace OrderService.Models;

public enum OrderStatus
{
    New = 0,
    Processing = 1,
    Completed = 2
}

public class Order
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public User Customer { get; set; }
    
    public List<Guid> ProductIds { get; set; }
    
    public List<Product> Products { get; set; }

    public decimal TotalAmount
    {
        get
        {
            return Products.Sum(p => p.Price);
        }
    }

    public DateTime OrderDate { get; set; }
    
    public OrderStatus Status { get; set;  }
}