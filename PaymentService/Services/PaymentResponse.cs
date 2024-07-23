namespace PaymentService.Services;

public class PaymentResponse
{
    public string ID { get; set; }
    public string UserID { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
    public string PaymentID { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string InvoiceID { get; set; }
}