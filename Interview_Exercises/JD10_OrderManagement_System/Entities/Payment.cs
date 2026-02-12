namespace JD10_OrderManagement_System.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public string Method { get; set; } = string.Empty; // CreditCard, BankTransfer, Cash

    public Order Order { get; set; } = null!;
}
