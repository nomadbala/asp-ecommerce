using System.ComponentModel.DataAnnotations;
using OrderService.Models;

namespace OrderService.Contracts;

public record UpdateOrderContract(
    [Required] List<Guid> ProductIds,
    [Required] OrderStatus Status
);