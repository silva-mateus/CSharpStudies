using VShop.Web.Models;

namespace VShop.Web.Services.Interfaces;

public interface ICouponService
{
    Task<CouponViewModel?> GetCouponByCodeAsync(string couponCode, string token);
}
