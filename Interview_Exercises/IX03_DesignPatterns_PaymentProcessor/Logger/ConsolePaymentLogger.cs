namespace IX03_DesignPatterns_PaymentProcessor.Logger;

public class ConsolePaymentLogger : IPaymentLogger
{
    public void LogPaymentAttempt(PaymentRequest request, PaymentResult result)
    {
        Console.WriteLine($"[Payment] Method={request.PaymentMethod}, Amount={request.Amount}, Currency={request.Currency}, Success={result.Success}, TransactionId={result.TransactionId}, Error={result.ErrorMessage}");
    }
}