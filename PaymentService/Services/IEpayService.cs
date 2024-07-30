using System.Security.Cryptography;

namespace PaymentService.Repositories;

public interface IEpayService
{
    Task<TokenResponse> GetTokenAsync();
    Task<RSA> GetPublicKeyAsync();
    Task<string> EncryptDataAsync();
    Task<PaymentResponse> MakePaymentAsync();
}