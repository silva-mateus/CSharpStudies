using VShop.DiscountApi.Domain.Entities;
using VShop.DiscountApi.Domain.Exceptions;

namespace VShop.DiscountApi.Domain.Tests;

public class CouponTests
{
    [Fact]
    public void Validate_WithValidData_DoesNotThrow()
    {
        var coupon = new Coupon { CouponCode = "PROMO10", Discount = 10 };
        coupon.Validate();
    }

    [Fact]
    public void Validate_WithEmptyCode_ThrowsDomainException()
    {
        var coupon = new Coupon { CouponCode = "", Discount = 10 };
        Assert.Throws<DomainException>(() => coupon.Validate());
    }

    [Fact]
    public void Validate_WithNullCode_ThrowsDomainException()
    {
        var coupon = new Coupon { CouponCode = null!, Discount = 10 };
        Assert.Throws<DomainException>(() => coupon.Validate());
    }

    [Fact]
    public void Validate_WithCodeTooLong_ThrowsDomainException()
    {
        var coupon = new Coupon { CouponCode = new string('A', 31), Discount = 10 };
        Assert.Throws<DomainException>(() => coupon.Validate());
    }

    [Fact]
    public void Validate_WithZeroDiscount_ThrowsDomainException()
    {
        var coupon = new Coupon { CouponCode = "PROMO", Discount = 0 };
        Assert.Throws<DomainException>(() => coupon.Validate());
    }

    [Fact]
    public void Validate_WithNegativeDiscount_ThrowsDomainException()
    {
        var coupon = new Coupon { CouponCode = "PROMO", Discount = -5 };
        Assert.Throws<DomainException>(() => coupon.Validate());
    }
}
