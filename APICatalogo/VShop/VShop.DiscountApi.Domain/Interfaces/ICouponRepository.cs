using VShop.DiscountApi.Domain.Entities;

namespace VShop.DiscountApi.Domain.Interfaces;

public interface ICouponRepository
{
    Task<Coupon?> GetByCodeAsync(string couponCode);
    Task<Coupon?> GetByIdAsync(int id);
    Task<IEnumerable<Coupon>> GetAllAsync();
    Task<Coupon> AddAsync(Coupon coupon);
    Task<Coupon> UpdateAsync(Coupon coupon);
    Task<bool> DeleteAsync(int id);
}
