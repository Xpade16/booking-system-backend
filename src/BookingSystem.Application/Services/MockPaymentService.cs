using Microsoft.Extensions.Logging;
using BookingSystem.Application.Services.Interfaces;

namespace BookingSystem.Infrastructure.Services;

public class MockPaymentService : IPaymentService
{
    private readonly ILogger<MockPaymentService> _logger;

    public MockPaymentService(ILogger<MockPaymentService> logger)
    {
        _logger = logger;
    }

    public Task<PaymentResult> ChargeAsync(decimal amount, string currency, string paymentToken)
    {
        _logger.LogInformation(
            "💳 MOCK PAYMENT: Charging {Amount} {Currency}\n" +
            "   Payment Token: {Token}\n" +
            "   Status: SUCCESS",
            amount, currency, paymentToken);
        
        // Simulate payment processing
        var result = new PaymentResult
        {
            Success = true,
            TransactionId = $"txn_{Guid.NewGuid():N}",
            ProcessedAt = DateTime.UtcNow,
            Amount = amount,
            Currency = currency
        };
        
        // Simulate 5% failure rate for testing
        if (new Random().Next(100) < 5)
        {
            result.Success = false;
            result.ErrorMessage = "Payment declined - Insufficient funds";
            _logger.LogWarning("💳 MOCK PAYMENT: Payment declined");
        }
        
        return Task.FromResult(result);
    }

    public Task<RefundResult> RefundAsync(string transactionId, decimal amount)
    {
        _logger.LogInformation(
            "💰 MOCK REFUND: Refunding {Amount} for transaction {TransactionId}",
            amount, transactionId);
        
        var result = new RefundResult
        {
            Success = true,
            RefundId = $"refund_{Guid.NewGuid():N}",
            RefundedAt = DateTime.UtcNow,
            Amount = amount
        };
        
        return Task.FromResult(result);
    }
}