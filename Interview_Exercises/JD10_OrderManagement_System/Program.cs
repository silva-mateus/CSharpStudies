using JD10_OrderManagement_System.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// TODO: Configure services:
// 1. Add DbContext with SQL Server LocalDB connection string
// 2. Register repositories (scoped)
// 3. Register services (scoped)
// 4. Add FluentValidation validators

var app = builder.Build();

// TODO: Ensure database is created (for development)
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
//     db.Database.EnsureCreated();
// }

// TODO: Map all endpoints:

// Customers:
// GET /customers -> list all
// GET /customers/{id} -> get by id
// POST /customers -> create (validate with FluentValidation)
// GET /customers/{id}/order-history?page=1&pageSize=10 -> paged order history

// Products:
// GET /products -> list (filter by ?category=, ?inStock=true)
// POST /products -> create
// PUT /products/{id} -> update

// Orders:
// GET /orders/{id} -> get with lines
// POST /orders -> create draft with lines
// POST /orders/{id}/submit -> validate stock, calculate totals, deduct stock
// POST /orders/{id}/cancel -> cancel and restore stock

// Reports:
// GET /reports/sales-summary?from=2025-01-01&to=2025-12-31 -> aggregated report

app.Run();

public partial class Program { }
