using Microsoft.EntityFrameworkCore;
using VShop.CartApi.Domain.Entities;

namespace VShop.CartApi.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<CartHeader> CartHeaders { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Product>().HasKey(p => p.Id);

        mb.Entity<Product>()
            .Property(p => p.Id)
            .ValueGeneratedNever();

        mb.Entity<Product>()
            .Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        mb.Entity<Product>()
            .Property(p => p.Description)
            .HasMaxLength(255)
            .IsRequired();

        mb.Entity<Product>()
            .Property(p => p.ImageURL)
            .HasMaxLength(255)
            .IsRequired();

        mb.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(12, 2)
            .IsRequired();

        mb.Entity<Product>()
            .Property(p => p.CategoryName)
            .HasMaxLength(100)
            .IsRequired();

        mb.Entity<CartHeader>()
            .Property(c => c.UserId)
            .HasMaxLength(255)
            .IsRequired();

        mb.Entity<CartHeader>()
            .Property(c => c.CouponCode)
            .HasMaxLength(100);
    }
}
