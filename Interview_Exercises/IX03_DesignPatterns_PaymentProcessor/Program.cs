using IX03_DesignPatterns_PaymentProcessor;
using IX03_DesignPatterns_PaymentProcessor.Logger;
using IX03_DesignPatterns_PaymentProcessor.Strategies;

// Composition root: wire up all strategies
var strategies = new IPaymentStrategy[]
{
    new CreditCardPaymentStrategy(),
    new BankTransferPaymentStrategy(),
    new CryptocurrencyPaymentStrategy()
};

var factory = new PaymentStrategyFactory(strategies);
var logger = new ConsolePaymentLogger();
var processor = new PaymentProcessor(factory, logger);

// Test a credit card payment
var ccRequest = new PaymentRequest(
    Amount: 99.99m,
    Currency: "USD",
    PaymentMethod: PaymentMethod.CreditCard,
    PaymentDetails: new Dictionary<string, string>
    {
        ["CardNumber"] = "4111111111111111",
        ["ExpiryDate"] = "12/27",
        ["CVV"] = "123"
    });

var result = processor.Process(ccRequest);
Console.WriteLine($"Credit Card: Success={result.Success}, TxId={result.TransactionId}");

// Test a bank transfer
var btRequest = new PaymentRequest(
    Amount: 5000m,
    Currency: "EUR",
    PaymentMethod: PaymentMethod.BankTransfer,
    PaymentDetails: new Dictionary<string, string>
    {
        ["IBAN"] = "DE89370400440532013000",
        ["BankCode"] = "COBADEFFXXX"
    });

result = processor.Process(btRequest);
Console.WriteLine($"Bank Transfer: Success={result.Success}, TxId={result.TransactionId}");

// Test an invalid crypto payment (short wallet address)
var cryptoRequest = new PaymentRequest(
    Amount: 0.5m,
    Currency: "BTC",
    PaymentMethod: PaymentMethod.Cryptocurrency,
    PaymentDetails: new Dictionary<string, string>
    {
        ["WalletAddress"] = "short",
        ["Network"] = "Bitcoin"
    });

result = processor.Process(cryptoRequest);
Console.WriteLine($"Crypto: Success={result.Success}, Error={result.ErrorMessage}");
