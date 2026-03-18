using Microsoft.EntityFrameworkCore;
using VShop.DiscountApi.Domain.Entities;

namespace VShop.DiscountApi.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Coupon>().HasKey(c => c.CouponId);

        mb.Entity<Coupon>()
            .Property(c => c.CouponCode)
            .HasMaxLength(30)
            .IsRequired();

        mb.Entity<Coupon>()
            .Property(c => c.Discount)
            .HasPrecision(10, 2)
            .IsRequired();

        mb.Entity<Coupon>().HasData(
            new Coupon { CouponId = 1, CouponCode = "VSHOP_PROMO_10", Discount = 10 },
            new Coupon { CouponId = 2, CouponCode = "VSHOP_PROMO_20", Discount = 20 }
        );
    }
}
