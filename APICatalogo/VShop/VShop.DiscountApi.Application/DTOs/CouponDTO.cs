namespace VShop.DiscountApi.Application.DTOs;

public class CouponDTO
{
    public int CouponId { get; set; }
    public string CouponCode { get; set; } = string.Empty;
    public decimal Discount { get; set; }
}
