# JD10 - Full-Stack Order Management System (Capstone)

## Difficulty: Hard
## Estimated Time: 90-120 minutes
## Type: Create from scratch (full stack with tests)

## Overview

Build a complete Order Management API from scratch, combining everything from the previous exercises: ASP.NET Core Minimal API, EF Core with SQL Server LocalDB, proper layered architecture, validation, and a comprehensive test suite. This is the capstone exercise that ties all skills together.

## Requirements

### Architecture

```
API Layer (Program.cs endpoints)
    |
Service Layer (OrderService, CustomerService)
    |
Repository Layer (EF Core DbContext, IOrderRepository, ICustomerRepository)
    |
SQL Server LocalDB
```

### Database Schema (design with EF Core + Fluent API)

| Entity | Properties |
|--------|-----------|
| Customer | Id, Name, Email (unique), Phone, CreatedAt |
| Product | Id, Name, Description, Price (> 0), StockQuantity (>= 0), Category |
| Order | Id, CustomerId (FK), OrderDate, Status (Draft/Submitted/Shipped/Cancelled), TotalAmount |
| OrderLine | Id, OrderId (FK), ProductId (FK), Quantity (> 0), UnitPrice, LineTotal |
| Payment | Id, OrderId (FK), Amount, PaymentDate, Method (CreditCard/BankTransfer/Cash) |

### Relationships
- Customer -> Orders: one-to-many
- Order -> OrderLines: one-to-many (cascade delete)
- Order -> Payments: one-to-many
- Product -> OrderLines: one-to-many (restrict delete)

### API Endpoints

#### Customers
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/customers` | List all customers |
| GET | `/customers/{id}` | Get customer details |
| POST | `/customers` | Create customer |
| GET | `/customers/{id}/order-history?page=&pageSize=` | Paged order history with totals |

#### Products
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/products` | List products (filter by `?category=`, `?inStock=true`) |
| POST | `/products` | Create product |
| PUT | `/products/{id}` | Update product |

#### Orders
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/orders/{id}` | Get order with lines |
| POST | `/orders` | Create draft order with lines |
| POST | `/orders/{id}/submit` | Submit order (validate stock, calculate totals, deduct stock) |
| POST | `/orders/{id}/cancel` | Cancel order (restore stock if was submitted) |

#### Reports
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/reports/sales-summary?from=&to=` | Aggregated: total orders, total revenue, avg order value, top products |

### Business Rules

- `POST /orders/{id}/submit`:
  1. Validate all order lines have sufficient stock.
  2. Calculate each line total (quantity * unit price).
  3. Calculate order total (sum of line totals).
  4. Deduct stock for each product.
  5. Set status to "Submitted".
  6. If any product has insufficient stock, reject the entire order.

- `POST /orders/{id}/cancel`:
  1. Only Submitted orders can be cancelled.
  2. Restore stock quantities.
  3. Set status to "Cancelled".

### Validation (FluentValidation)
- Customer: Name required, Email required + valid format.
- Product: Name required, Price > 0, StockQuantity >= 0.
- Order: Must have at least 1 line, each line Quantity > 0.

## Tests to Write (20+)

### Unit Tests (service layer, 8+)
1. Submit order calculates totals correctly.
2. Submit order deducts stock.
3. Submit order with insufficient stock fails.
4. Cancel order restores stock.
5. Cancel draft order fails (only submitted can be cancelled).
6. Order total equals sum of line totals.
7. Customer order history is paged correctly.
8. Sales summary aggregation is correct.

### Integration Tests (API endpoints, 8+)
1. POST /customers creates customer (201).
2. POST /products creates product (201).
3. POST /orders creates draft order (201).
4. POST /orders/{id}/submit succeeds and deducts stock.
5. POST /orders/{id}/submit with insufficient stock returns 400.
6. POST /orders/{id}/cancel restores stock.
7. GET /customers/{id}/order-history returns paged results.
8. GET /reports/sales-summary returns correct aggregation.

### Database Tests (4+)
1. EF Core creates schema correctly (EnsureCreated).
2. Cascade delete removes OrderLines when Order is deleted.
3. Unique constraint on Customer.Email is enforced.
4. Check constraint on Product.Price > 0 is enforced.

## Constraints

- Use ASP.NET Core Minimal APIs.
- Use EF Core with SQL Server LocalDB.
- Use FluentValidation for input validation.
- Use proper layered architecture (no business logic in endpoints).
- Integration tests use `WebApplicationFactory` with a test database.
- Connection string: `Server=(localdb)\\MSSQLLocalDB;Database=JD10_OrderDB;Trusted_Connection=True;`

## Topics Covered

- ASP.NET Core Minimal APIs
- EF Core (Fluent API, relationships, migrations)
- SQL Server LocalDB
- Repository pattern
- Service layer with business logic
- FluentValidation
- Pagination
- Aggregation queries
- Unit testing with fakes
- Integration testing with WebApplicationFactory
- Database integration testing
