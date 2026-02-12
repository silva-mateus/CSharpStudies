namespace IX03_DesignPatterns_PaymentProcessor;

public class PaymentStrategyFactory
{
    private readonly IEnumerable<IPaymentStrategy> _strategies;

    public PaymentStrategyFactory(IEnumerable<IPaymentStrategy> strategies)
    {
        _strategies = strategies.ToList();
    }

    /// <summary>
    /// Returns the strategy that supports the given payment method.
    /// Throws <see cref="NotSupportedException"/> if no matching strategy is found.
    /// </summary>
    public IPaymentStrategy GetStrategy(PaymentMethod method)
    {
        var strategy = _strategies.FirstOrDefault(s => s.SupportedMethod == method);
        return strategy ?? throw new NotSupportedException("Method not supported");
    }
}
