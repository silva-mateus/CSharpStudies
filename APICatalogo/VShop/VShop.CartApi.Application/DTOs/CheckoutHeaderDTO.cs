namespace VShop.CartApi.Application.DTOs;

public class CheckoutHeaderDTO
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string CouponCode { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string CardNumber { get; set; } = string.Empty;
    public string NameOnCard { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;
    public string ExpiryMonthYear { get; set; } = string.Empty;

    public DateTime DateTime { get; set; }

    public int CartTotalItems { get; set; }
    public IEnumerable<CartItemDTO> CartItems { get; set; } = Enumerable.Empty<CartItemDTO>();
}
