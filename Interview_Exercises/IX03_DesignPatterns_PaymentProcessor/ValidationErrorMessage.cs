namespace IX03_DesignPatterns_PaymentProcessor;

public static class ValidationErrorMessage
{
    // Credit Card
    public const string CardNumberRequired = "CardNumber is required";
    public const string CardNumberMustBe16Digits = "CardNumber must have 16 digits";
    public const string ExpiryDateRequired = "ExpiryDate is required";
    public const string ExpiryDateInvalidFormat = "ExpiryDate must have the following format MM/YY";
    public const string CvvRequired = "CVV is required";
    public const string CvvMustBe3Digits = "CVV must have 3 digits";
    public const string AmountCannotExceed10000 = "Amount cannot exceed 10,000";

    // Bank Transfer
    public const string IbanRequired = "IBAN is required";
    public const string IbanInvalidFormat = "IBAN must start with 2 letters followed by digits";
    public const string BankCodeRequired = "BankCode is required";
    public const string BankCodeShouldNotBeEmpty = "BankCode should not be empty";
    public const string AmountCannotBeUnder1 = "Amount cannot be under 1.00";

    // Cryptocurrency
    public const string WalletAddressRequired = "WalletAddress is required";
    public const string WalletAddressMinLength = "WalletAddress must have 26 characters";
    public const string NetworkRequired = "Network is required";
    public const string NetworkNotSupported = "Network must be one of the following: Bitcoin, Ethereum, Solana";

}
