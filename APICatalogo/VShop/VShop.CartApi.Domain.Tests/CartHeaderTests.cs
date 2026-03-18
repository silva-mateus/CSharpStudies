using VShop.CartApi.Domain.Entities;
using VShop.CartApi.Domain.Exceptions;

namespace VShop.CartApi.Domain.Tests;

public class CartHeaderTests
{
    [Fact]
    public void ApplyCoupon_WithValidCode_SetsCouponCode()
    {
        var header = new CartHeader { UserId = "user1" };
        header.ApplyCoupon("PROMO10");
        Assert.Equal("PROMO10", header.CouponCode);
    }

    [Fact]
    public void ApplyCoupon_WithEmptyCode_ThrowsDomainException()
    {
        var header = new CartHeader { UserId = "user1" };
        Assert.Throws<DomainException>(() => header.ApplyCoupon(""));
    }

    [Fact]
    public void ApplyCoupon_WithNullCode_ThrowsDomainException()
    {
        var header = new CartHeader { UserId = "user1" };
        Assert.Throws<DomainException>(() => header.ApplyCoupon(null!));
    }

    [Fact]
    public void RemoveCoupon_ClearsCouponCode()
    {
        var header = new CartHeader { UserId = "user1", CouponCode = "PROMO10" };
        header.RemoveCoupon();
        Assert.Equal(string.Empty, header.CouponCode);
    }
}
