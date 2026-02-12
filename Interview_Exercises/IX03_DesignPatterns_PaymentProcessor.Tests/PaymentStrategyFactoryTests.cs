using Xunit;
using FluentAssertions;
using IX03_DesignPatterns_PaymentProcessor;
using IX03_DesignPatterns_PaymentProcessor.Strategies;
namespace IX03_DesignPatterns_PaymentProcessor.Tests;


public class PaymentStrategyFactoryTests
{
    private readonly PaymentStrategyFactory _factory;

    private readonly IPaymentStrategy[] _strategies = new IPaymentStrategy[]
    {
        new CreditCardPaymentStrategy(),
        new BankTransferPaymentStrategy(),
        new CryptocurrencyPaymentStrategy()
    };

    public PaymentStrategyFactoryTests()
    {
        _factory = new PaymentStrategyFactory(_strategies);
    }

    [Theory]
    [InlineData(PaymentMethod.CreditCard)]
    [InlineData(PaymentMethod.BankTransfer)]
    [InlineData(PaymentMethod.Cryptocurrency)]
    public void GetStrategy_ShouldReturnCorrectStrategy_ForEachPaymentMethod(PaymentMethod method)
    {
        var strategy = _factory.GetStrategy(method);

        strategy.Should().NotBeNull();
        strategy.SupportedMethod.Should().Be(method);
    }

    [Fact]
    public void GetStrategy_ShouldThrowNotSupportedException_ForUnknownPaymentMethod()
    {
        var factoryWithOnlyCreditCardStrategy = new PaymentStrategyFactory(new IPaymentStrategy[] { new CreditCardPaymentStrategy() });
        
        var action = () => factoryWithOnlyCreditCardStrategy.GetStrategy(PaymentMethod.BankTransfer);
        action.Should().Throw<NotSupportedException>();
    }

}