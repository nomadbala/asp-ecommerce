using System.Security.Cryptography;

namespace PaymentService.Repositories;

public interface IEpayService
{
    Task<TokenResponse> GetTokenAsync(string invoiceId);
    Task<RSA> GetPublicKeyAsync();
    Task<string> EncryptDataAsync();
    Task<PaymentResponse> MakePaymentAsync();
}