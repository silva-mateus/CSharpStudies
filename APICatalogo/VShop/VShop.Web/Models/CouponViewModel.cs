namespace VShop.Web.Models;

public class CouponViewModel
{
    public int CouponId { get; set; }
    public string CouponCode { get; set; } = string.Empty;
    public decimal Discount { get; set; }
}
