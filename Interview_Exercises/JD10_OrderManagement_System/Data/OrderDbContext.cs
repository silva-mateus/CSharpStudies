using JD10_OrderManagement_System.Entities;
using Microsoft.EntityFrameworkCore;

namespace JD10_OrderManagement_System.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO: Configure all entities using Fluent API:
        //
        // Customer:
        //   - Name required, max 100
        //   - Email required, unique, max 200
        //
        // Product:
        //   - Name required, max 200
        //   - Price has CHECK (> 0) or configure precision
        //   - StockQuantity >= 0
        //   - Category required, max 100
        //
        // Order:
        //   - Has one Customer (many orders)
        //   - Has many OrderLines (cascade delete)
        //   - Has many Payments (cascade delete)
        //   - Status max 20, default "Draft"
        //   - TotalAmount precision (18,2)
        //
        // OrderLine:
        //   - Has one Order (cascade delete)
        //   - Has one Product (restrict delete - can't delete product with order lines)
        //   - Quantity > 0
        //   - UnitPrice and LineTotal precision (18,2)
        //
        // Payment:
        //   - Has one Order
        //   - Amount precision (18,2)
        //   - Method max 20

        throw new NotImplementedException("Configure Fluent API here");
    }
}
