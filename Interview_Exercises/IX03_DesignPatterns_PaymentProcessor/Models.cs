namespace IX03_DesignPatterns_PaymentProcessor;

public enum PaymentMethod
{
    CreditCard,
    BankTransfer,
    Cryptocurrency
}

public enum CryptoNetwork
{
    Bitcoin,
    Ethereum,
    Solana
}

public record PaymentRequest(
    decimal Amount,
    string Currency,
    PaymentMethod PaymentMethod,
    Dictionary<string, string> PaymentDetails);

public record PaymentResult(
    bool Success,
    string? TransactionId = null,
    string? ErrorMessage = null);

public record ValidationResult(bool IsValid, List<string> Errors)
{
    public static ValidationResult Ok() => new(true, new List<string>());

    public static ValidationResult Fail(params string[] errors) =>
        new(false, errors.ToList());
}

