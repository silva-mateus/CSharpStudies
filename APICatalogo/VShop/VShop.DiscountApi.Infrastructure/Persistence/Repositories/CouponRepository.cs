using Microsoft.EntityFrameworkCore;
using VShop.DiscountApi.Domain.Entities;
using VShop.DiscountApi.Domain.Interfaces;

namespace VShop.DiscountApi.Infrastructure.Persistence.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly AppDbContext _context;

    public CouponRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Coupon?> GetByCodeAsync(string couponCode)
    {
        return await _context.Coupons
            .FirstOrDefaultAsync(c => c.CouponCode == couponCode);
    }

    public async Task<Coupon?> GetByIdAsync(int id)
    {
        return await _context.Coupons
            .FirstOrDefaultAsync(c => c.CouponId == id);
    }

    public async Task<IEnumerable<Coupon>> GetAllAsync()
    {
        return await _context.Coupons.ToListAsync();
    }

    public async Task<Coupon> AddAsync(Coupon coupon)
    {
        _context.Coupons.Add(coupon);
        await _context.SaveChangesAsync();
        return coupon;
    }

    public async Task<Coupon> UpdateAsync(Coupon coupon)
    {
        _context.Coupons.Update(coupon);
        await _context.SaveChangesAsync();
        return coupon;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.CouponId == id);

        if (coupon is null) return false;

        _context.Coupons.Remove(coupon);
        await _context.SaveChangesAsync();
        return true;
    }
}
