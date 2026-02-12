namespace IX03_DesignPatterns_PaymentProcessor.Strategies;

public class CryptocurrencyPaymentStrategy : IPaymentStrategy
{
    public PaymentMethod SupportedMethod => PaymentMethod.Cryptocurrency;

    public ValidationResult Validate(PaymentRequest request)
    {
        var errors = new List<string>();

        if (!request.PaymentDetails.TryGetValue("WalletAddress", out var walletAddress))
            errors.Add(ValidationErrorMessage.WalletAddressRequired);
        else if (!IsValidWalletAddress(walletAddress))
            errors.Add(ValidationErrorMessage.WalletAddressMinLength);

        if (!request.PaymentDetails.TryGetValue("Network", out var network))
            errors.Add(ValidationErrorMessage.NetworkRequired);
        else if (!IsValidNetwork(network))
            errors.Add(ValidationErrorMessage.NetworkNotSupported);

        if (errors.Count > 0)
            return ValidationResult.Fail(errors.ToArray());
        
        return ValidationResult.Ok();
    }
    private bool IsValidWalletAddress(string? walletAddress) => !string.IsNullOrEmpty(walletAddress) && walletAddress.Length >= 26 && walletAddress.All(char.IsLetterOrDigit);
    private bool IsValidNetwork(string? network) =>
        !string.IsNullOrEmpty(network) &&
        Enum.TryParse<CryptoNetwork>(network, ignoreCase: true, out _);
        
}
