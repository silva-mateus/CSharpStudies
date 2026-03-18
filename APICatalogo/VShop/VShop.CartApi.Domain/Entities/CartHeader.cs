using VShop.CartApi.Domain.Exceptions;

namespace VShop.CartApi.Domain.Entities;

public class CartHeader
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string CouponCode { get; set; } = string.Empty;

    public void ApplyCoupon(string couponCode)
    {
        if (string.IsNullOrWhiteSpace(couponCode))
            throw new DomainException("Coupon code cannot be empty.");

        CouponCode = couponCode;
    }

    public void RemoveCoupon()
    {
        CouponCode = string.Empty;
    }
}
