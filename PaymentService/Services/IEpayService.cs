namespace PaymentService.Services;

public interface IEpayService
{
    Task<string> TokenAsync();
    Task<PaymentResponse> PayAsync();
}