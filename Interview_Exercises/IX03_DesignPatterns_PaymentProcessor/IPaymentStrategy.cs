namespace IX03_DesignPatterns_PaymentProcessor;

public interface IPaymentValidator
{
    ValidationResult Validate(PaymentRequest request);
}

public interface IPaymentStrategy
{
    PaymentMethod SupportedMethod { get; }
    ValidationResult Validate(PaymentRequest request);
    PaymentResult ProcessPayment(PaymentRequest request) => new PaymentResult(true, TransactionIdPrefix.For(SupportedMethod) + Guid.NewGuid().ToString());

}
