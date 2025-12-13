namespace BookingSystem.Application.Services.Interfaces;

public interface IPaymentService
{
    Task<PaymentResult> ChargeAsync(decimal amount, string currency, string paymentToken);
    Task<RefundResult> RefundAsync(string transactionId, decimal amount);
}

public class PaymentResult
{
    public bool Success { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}

public class RefundResult
{
    public bool Success { get; set; }
    public string RefundId { get; set; } = string.Empty;
    public DateTime RefundedAt { get; set; }
    public decimal Amount { get; set; }
    public string? ErrorMessage { get; set; }
}