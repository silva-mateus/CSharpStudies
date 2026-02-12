namespace IX03_DesignPatterns_PaymentProcessor.Logger;

public interface IPaymentLogger
{
    void LogPaymentAttempt(PaymentRequest request, PaymentResult result);
}