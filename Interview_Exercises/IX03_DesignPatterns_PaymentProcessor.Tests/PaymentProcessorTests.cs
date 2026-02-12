using IX03_DesignPatterns_PaymentProcessor;
using IX03_DesignPatterns_PaymentProcessor.Logger;
using IX03_DesignPatterns_PaymentProcessor.Strategies;
using Xunit;
using FluentAssertions;

namespace IX03_DesignPatterns_PaymentProcessor.Tests;

public class PaymentProcessorTests
{
    private readonly PaymentStrategyFactory _factory;
    private readonly IPaymentLogger _logger;
    private readonly PaymentProcessor _processor;

    private readonly IPaymentStrategy[] _strategies = new IPaymentStrategy[]
    {
        new CreditCardPaymentStrategy(),
        new BankTransferPaymentStrategy(),
        new CryptocurrencyPaymentStrategy()
    };

    public PaymentProcessorTests()
    {
        _factory = new PaymentStrategyFactory(_strategies);
        _logger = new FakePaymentLogger();
        _processor = new PaymentProcessor(_factory, _logger);
    }


    [Theory]
    [InlineData("1111111111111111", "01/25", "123")]
    [InlineData("2111111111111111", "06/30", "999")]
    [InlineData("3111111111111111", "12/35", "543")]
    public void Process_ShouldReturnSuccess_OnValidCreditCardPayment(string cardNumber, string expiryDate, string cvv)
    {
        var ccRequest = new PaymentRequest(
            Amount: 99.99m,
            Currency: "USD",
            PaymentMethod: PaymentMethod.CreditCard,
            PaymentDetails: new Dictionary<string, string>
            {
                ["CardNumber"] = cardNumber,
                ["ExpiryDate"] = expiryDate,
                ["CVV"] = cvv
            });

        var result = _processor.Process(ccRequest);

        result.Success.Should().BeTrue();
        result.TransactionId.Should().StartWith(TransactionIdPrefix.For(PaymentMethod.CreditCard));
    }

    [Fact]
    public void Process_ShouldReturnValidationError_OnCreditCardPaymentWithMissingCardNumber()
    {
        var ccRequest = new PaymentRequest(
            Amount: 99.99m,
            Currency: "USD",
            PaymentMethod: PaymentMethod.CreditCard,
            PaymentDetails: new Dictionary<string, string>
            {
                ["ExpiryDate"] = "12/27",
                ["CVV"] = "123"
            });

        var result = _processor.Process(ccRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.CardNumberRequired);
    }

    [Fact]
    public void Process_ShouldReturnValidationError_OnCreditCardPaymentWithMissingExpiryDate()
    {
        var ccRequest = new PaymentRequest(
            Amount: 99.99m,
            Currency: "USD",
            PaymentMethod: PaymentMethod.CreditCard,
            PaymentDetails: new Dictionary<string, string>
            {
                ["CardNumber"] = "4111111111111111",
                ["CVV"] = "123"
            });

        var result = _processor.Process(ccRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.ExpiryDateRequired);

    }

    [Fact]
    public void Process_ShouldReturnValidationError_OnCreditCardPaymentWithMissingCVV()
    {
        var ccRequest = new PaymentRequest(
            Amount: 99.99m,
            Currency: "USD",
            PaymentMethod: PaymentMethod.CreditCard,
            PaymentDetails: new Dictionary<string, string>
            {
                ["CardNumber"] = "4111111111111111",
                ["ExpiryDate"] = "12/27",
            });

        var result = _processor.Process(ccRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.CvvRequired);
    }

    [Theory]
    [InlineData("A111111111111111")]
    [InlineData("123456")]
    [InlineData("411111111111111111")]
    [InlineData("AAAAAAAAAAAAAAAA")]
    public void Process_ShouldReturnValidationError_OnCreditCardPaymentWithInvalidCardNumber(string cardNumber)
    {
        var ccRequest = new PaymentRequest(
            Amount: 99.99m,
            Currency: "USD",
            PaymentMethod: PaymentMethod.CreditCard,
            PaymentDetails: new Dictionary<string, string>
            {
                ["CardNumber"] = cardNumber,
                ["ExpiryDate"] = "12/27",
                ["CVV"] = "123"
            });

        var result = _processor.Process(ccRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.CardNumberMustBe16Digits);
    }

    [Fact]
    public void Process_ShouldReturnValidationError_OnCreditCardPaymentWithAmountOver10_000()
    {
        // Test a credit card payment
        var ccRequest = new PaymentRequest(
            Amount: 11_000m,
            Currency: "USD",
            PaymentMethod: PaymentMethod.CreditCard,
            PaymentDetails: new Dictionary<string, string>
            {
                ["CardNumber"] = "4111111111111111",
                ["ExpiryDate"] = "12/27",
                ["CVV"] = "123"
            });

        var result = _processor.Process(ccRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.AmountCannotExceed10000);
    }

    [Theory]
    [InlineData("MM/YY")]
    [InlineData("12/YY")]
    [InlineData("13/27")]
    [InlineData("12/2027")]
    [InlineData("12-27")]
    [InlineData("1227")]
    public void Process_ShouldReturnValidationError_OnCreditCardPaymentWithIncorrectExpiryDate(string expiryDate)
    {
        // Test a credit card payment
        var ccRequest = new PaymentRequest(
            Amount: 11_000m,
            Currency: "USD",
            PaymentMethod: PaymentMethod.CreditCard,
            PaymentDetails: new Dictionary<string, string>
            {
                ["CardNumber"] = "4111111111111111",
                ["ExpiryDate"] = expiryDate,
                ["CVV"] = "123"
            });

        var result = _processor.Process(ccRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.ExpiryDateInvalidFormat);
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("A23")]
    [InlineData("ABC")]
    [InlineData("12")]
    public void Process_ShouldReturnValidationError_OnCreditCardPaymentWithInvalidCVV(string cvv)
    {
        var ccRequest = new PaymentRequest(
            Amount: 11_000m,
            Currency: "USD",
            PaymentMethod: PaymentMethod.CreditCard,
            PaymentDetails: new Dictionary<string, string>
            {
                ["CardNumber"] = "4111111111111111",
                ["ExpiryDate"] = "12/27",
                ["CVV"] = cvv
            });

        var result = _processor.Process(ccRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.CvvMustBe3Digits);
    }

    [Fact]
    public void Process_ShouldReturnSuccess_OnValidBankTransferPayment()
    {
        var btRequest = new PaymentRequest(
            Amount: 5000m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.BankTransfer,
            PaymentDetails: new Dictionary<string, string>
            {
                ["IBAN"] = "DE89370400440532013000",
                ["BankCode"] = "COBADEFFXXX"
            });

        var result = _processor.Process(btRequest);


        result.Success.Should().BeTrue();
        result.TransactionId.Should().StartWith(TransactionIdPrefix.For(PaymentMethod.BankTransfer));
    }

    [Fact]
    public void Process_ShouldReturnValidationError_OnBankTransferPaymentWithMissingIBAN()
    {
        var btRequest = new PaymentRequest(
            Amount: 5000m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.BankTransfer,
            PaymentDetails: new Dictionary<string, string>
            {
                ["BankCode"] = "COBADEFFXXX"
            });

        var result = _processor.Process(btRequest);


        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.IbanRequired);
    }

    [Fact]
    public void Process_ShouldReturnValidationError_OnBankTransferPaymentWithMissingBankCode()
    {
        var btRequest = new PaymentRequest(
            Amount: 5000m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.BankTransfer,
            PaymentDetails: new Dictionary<string, string>
            {
                ["IBAN"] = "DE89370400440532013000"
            });

        var result = _processor.Process(btRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.BankCodeRequired);
    }

    [Theory]
    [InlineData("DE8937")]
    [InlineData("1289370400440532013000")]
    [InlineData(null)]
    public void Process_ShouldReturnValidationError_OnBankTransferPaymentWithInvalidIBAN(string? iban)
    {
        var btRequest = new PaymentRequest(
            Amount: 5000m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.BankTransfer,
            PaymentDetails: new Dictionary<string, string>
            {
                ["IBAN"] = iban!,
                ["BankCode"] = "COBADEFFXXX"
            });

        var result = _processor.Process(btRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.IbanInvalidFormat);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData(null)]
    public void Process_ShouldReturnValidationError_OnBankTransferPaymentWithInvalidBankCode(string? bankCode)
    {
        var btRequest = new PaymentRequest(
            Amount: 5000m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.BankTransfer,
            PaymentDetails: new Dictionary<string, string>
            {
                ["IBAN"] = "DE89370400440532013000",
                ["BankCode"] = bankCode!,
            });

        var result = _processor.Process(btRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.BankCodeShouldNotBeEmpty);
    }

    [Fact]
    public void Process_ShouldReturnValidationError_OnBankTransferPaymentWithAmountUnder1()
    {
        var btRequest = new PaymentRequest(
            Amount: 0.99m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.BankTransfer,
            PaymentDetails: new Dictionary<string, string>
            {
                ["IBAN"] = "DE89370400440532013000",
                ["BankCode"] = "COBADEFFXXX"
            });

        var result = _processor.Process(btRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.AmountCannotBeUnder1);
    }

    [Fact]
    public void Process_ShouldReturnSuccess_OnValidCryptocurrencyPayment()
    {
        var cryptoRequest = new PaymentRequest(
            Amount: 5000m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.Cryptocurrency,
            PaymentDetails: new Dictionary<string, string>
            {
                ["WalletAddress"] = "0x1234567890123456789012345678901234567890",
                ["Network"] = "Bitcoin"
            });

        var result = _processor.Process(cryptoRequest);

        result.Success.Should().BeTrue();
        result.TransactionId.Should().StartWith(TransactionIdPrefix.For(PaymentMethod.Cryptocurrency));
    }

    [Fact]
    public void Process_ShouldReturnValidationError_OnCryptocurrencyPaymentWithMissingWalletAddress()
    {
        var cryptoRequest = new PaymentRequest(
            Amount: 5000m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.Cryptocurrency,
            PaymentDetails: new Dictionary<string, string>
            {
                ["Network"] = "Bitcoin"
            });

        var result = _processor.Process(cryptoRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.WalletAddressRequired);
    }

    [Fact]
    public void Process_ShouldReturnValidationError_OnCryptocurrencyPaymentWithMissingNetwork()
    {
        var cryptoRequest = new PaymentRequest(
            Amount: 5000m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.Cryptocurrency,
            PaymentDetails: new Dictionary<string, string>
            {
                ["WalletAddress"] = "0x1234567890123456789012345678901234567890"
            });

        var result = _processor.Process(cryptoRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.NetworkRequired);
    }

    [Theory]
    [InlineData("short")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("0?1234567890123456789012345678901234567890")]
    public void Process_ShouldReturnValidationError_OnCryptocurrencyPaymentWithInvalidWalletAddress(string? walletAddress)
    {
        var cryptoRequest = new PaymentRequest(
            Amount: 5000m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.Cryptocurrency,
            PaymentDetails: new Dictionary<string, string>
            {
                ["WalletAddress"] = walletAddress!,
                ["Network"] = "Bitcoin"
            });

        var result = _processor.Process(cryptoRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.WalletAddressMinLength);
    }

    [Theory]
    [InlineData("Mycoin")]
    [InlineData("testcoin")]
    [InlineData("")]
    [InlineData(null)]
    public void Process_ShouldReturnValidationError_OnCryptocurrencyPaymentWithInvalidNetwork(string? network)
    {
        var cryptoRequest = new PaymentRequest(
            Amount: 5000m,
            Currency: "EUR",
            PaymentMethod: PaymentMethod.Cryptocurrency,
            PaymentDetails: new Dictionary<string, string>
            {
                ["WalletAddress"] = "0x1234567890123456789012345678901234567890",
                ["Network"] = network!,
            });

        var result = _processor.Process(cryptoRequest);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain(ValidationErrorMessage.NetworkNotSupported);
    }
}
