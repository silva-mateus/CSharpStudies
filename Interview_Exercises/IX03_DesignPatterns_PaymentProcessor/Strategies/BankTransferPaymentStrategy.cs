using System.Text.RegularExpressions;
namespace IX03_DesignPatterns_PaymentProcessor.Strategies;

public class BankTransferPaymentStrategy : IPaymentStrategy
{
    public PaymentMethod SupportedMethod => PaymentMethod.BankTransfer;

    public ValidationResult Validate(PaymentRequest request)
    {
        var errors = new List<string>();

        if (!request.PaymentDetails.TryGetValue("IBAN", out var iban))
            errors.Add(ValidationErrorMessage.IbanRequired);
        else if (!IsValidIBAN(iban))
            errors.Add(ValidationErrorMessage.IbanInvalidFormat);
    
        if (!request.PaymentDetails.TryGetValue("BankCode", out var bankCode))
            errors.Add(ValidationErrorMessage.BankCodeRequired);
        else if (string.IsNullOrEmpty(bankCode?.Trim()))
            errors.Add(ValidationErrorMessage.BankCodeShouldNotBeEmpty);

        if (request.Amount < 1m)
            errors.Add(ValidationErrorMessage.AmountCannotBeUnder1);

        if (errors.Count > 0)
            return ValidationResult.Fail(errors.ToArray());

        return ValidationResult.Ok();
    }
    private bool IsValidIBAN(string? iban) => !string.IsNullOrEmpty(iban) && iban.Length >= 15 && Regex.IsMatch(iban, @"^[A-Z]{2}\d*$");
}
