﻿using System.ComponentModel.DataAnnotations;
using PaymentService.Models;

namespace PaymentService.Contracts;

public record CreatePaymentContract(
    [Required] Guid UserId,
    [Required] Guid OrderId,
    [Required] decimal Amount,
    [Required] PaymentStatus Status
);