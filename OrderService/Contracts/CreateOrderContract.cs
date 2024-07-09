using System.ComponentModel.DataAnnotations;
using OrderService.Models;

namespace OrderService.Contracts;

public record CreateOrderContract(
    [Required] Guid CustomerId,
    [Required] List<Guid> ProductIds,
    [Required] OrderStatus Status
);