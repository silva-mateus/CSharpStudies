using VShop.DiscountApi.Domain.Exceptions;

namespace VShop.DiscountApi.Domain.Entities;

public class Coupon
{
    public int CouponId { get; set; }
    public string CouponCode { get; set; } = string.Empty;
    public decimal Discount { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(CouponCode))
            throw new DomainException("Coupon code is required.");

        if (CouponCode.Length > 30)
            throw new DomainException("Coupon code must be at most 30 characters.");

        if (Discount <= 0)
            throw new DomainException("Discount must be greater than zero.");
    }
}
