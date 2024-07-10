using System.ComponentModel.DataAnnotations;
using PaymentService.Models;

namespace PaymentService.Contracts;

public record UpdatePaymentContract(
    [Required] decimal Amount,
    [Required] PaymentStatus Status
);