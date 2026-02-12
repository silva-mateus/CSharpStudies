namespace IX03_DesignPatterns_PaymentProcessor;

public static class TransactionIdPrefix
{
    public static string For(PaymentMethod method) => method switch
    {
        PaymentMethod.CreditCard => "CC-",
        PaymentMethod.BankTransfer => "BT-",
        PaymentMethod.Cryptocurrency => "CRYPTO-",
        _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
    };
}