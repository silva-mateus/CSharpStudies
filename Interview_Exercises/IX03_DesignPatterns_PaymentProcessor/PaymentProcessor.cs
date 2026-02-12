using IX03_DesignPatterns_PaymentProcessor.Logger;

namespace IX03_DesignPatterns_PaymentProcessor;

public class PaymentProcessor
{
    private readonly PaymentStrategyFactory _factory;
    private readonly IPaymentLogger _logger;


    public PaymentProcessor(PaymentStrategyFactory factory, IPaymentLogger logger)
    {
        _factory = factory;
        _logger = logger;
    }

    /// <summary>
    /// Processes a payment request by:
    /// 1. Resolving the correct strategy via the factory.
    /// 2. Validating the request.
    /// 3. If valid, processing the payment.
    /// 4. If invalid, returning a failed result with error details.
    /// </summary>
    public PaymentResult Process(PaymentRequest request)
    {
        PaymentResult result;
        IPaymentStrategy strategy = _factory.GetStrategy(request.PaymentMethod);

        ValidationResult validationResult = strategy.Validate(request);

        if (!validationResult.IsValid)
        {
            string errorDetails = string.Join("; ", validationResult.Errors);
            result = new PaymentResult(false, null, errorDetails);
        }
        else
        {
            result = strategy.ProcessPayment(request);
        }

        _logger.LogPaymentAttempt(request, result);
        return result;
    }
}
