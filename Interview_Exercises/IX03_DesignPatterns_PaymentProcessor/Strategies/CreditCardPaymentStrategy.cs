using System.Text.RegularExpressions;

namespace IX03_DesignPatterns_PaymentProcessor.Strategies;

public class CreditCardPaymentStrategy : IPaymentStrategy
{
    public PaymentMethod SupportedMethod => PaymentMethod.CreditCard;

    public ValidationResult Validate(PaymentRequest request)
    {
        var errors = new List<string>();

        if (!request.PaymentDetails.TryGetValue("CardNumber", out var cardNumber))
            errors.Add(ValidationErrorMessage.CardNumberRequired);
        else if (!IsValidCardNumber(cardNumber))
            errors.Add(ValidationErrorMessage.CardNumberMustBe16Digits);
        
        if (!request.PaymentDetails.TryGetValue("ExpiryDate", out var expiryDate))
            errors.Add(ValidationErrorMessage.ExpiryDateRequired);
        else if (!IsValidExpiryDate(expiryDate))
            errors.Add(ValidationErrorMessage.ExpiryDateInvalidFormat);

        if (!request.PaymentDetails.TryGetValue("CVV", out var cvv))
            errors.Add(ValidationErrorMessage.CvvRequired);
        else if (!IsValidCVV(cvv))
            errors.Add(ValidationErrorMessage.CvvMustBe3Digits);

        if (request.Amount > 10_000m)
            errors.Add(ValidationErrorMessage.AmountCannotExceed10000);

        if (errors.Count > 0)
            return ValidationResult.Fail(errors.ToArray());
        
        return ValidationResult.Ok();
    }
    private bool IsValidCardNumber(string? cardNumber) => !string.IsNullOrEmpty(cardNumber?.Trim()) && cardNumber.Length == 16 && cardNumber.All(char.IsDigit);
    private bool IsValidExpiryDate(string? expiryDate) => !string.IsNullOrEmpty(expiryDate?.Trim()) && Regex.IsMatch(expiryDate, @"^(0[1-9]|1[0-2])/\d{2}$");
    private bool IsValidCVV(string? cvv) => !string.IsNullOrEmpty(cvv?.Trim()) && cvv.Length == 3 && cvv.All(char.IsDigit);
    
}
