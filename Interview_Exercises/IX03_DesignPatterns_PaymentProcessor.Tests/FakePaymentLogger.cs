namespace IX03_DesignPatterns_PaymentProcessor.Tests;
using IX03_DesignPatterns_PaymentProcessor.Logger;

public class FakePaymentLogger : IPaymentLogger
{
    public List<string> OutputLogs = new();
    public void LogPaymentAttempt(PaymentRequest request, PaymentResult result)
    {
        OutputLogs.Add($"[Payment] Method={request.PaymentMethod}, Amount={request.Amount}, Currency={request.Currency}, Success={result.Success}, TransactionId={result.TransactionId}, Error={result.ErrorMessage}");
    }
}