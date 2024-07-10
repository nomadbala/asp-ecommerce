using PaymentService.Contracts;
using PaymentService.Models;

namespace PaymentService.Services;

public interface IEpayService
{
    Task<string> GetPaymentTokenAsync();
    Task<string> EncryptDataAsync(object data);
    Task<PaymentResponse> MakePaymentAsync(PaymentRequest request, string token);
}