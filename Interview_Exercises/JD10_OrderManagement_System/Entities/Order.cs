namespace JD10_OrderManagement_System.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Draft"; // Draft, Submitted, Shipped, Cancelled
    public decimal TotalAmount { get; set; }

    public Customer Customer { get; set; } = null!;
    public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
