namespace PaymentService.Contracts;

public class PaymentResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
    public string PaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string InvoiceId { get; set; }
}