namespace PaymentService.Contracts;

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string CardHolderName { get; set; }
    public string InvoiceId { get; set; }
    public string InvoiceIdAlt { get; set; }
    public string Description { get; set; }
    public string AccountId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool CardSave { get; set; }
    public string Data { get; set; }
    public string PostLink { get; set; }
    public string FailurePostLink { get; set; }
}
