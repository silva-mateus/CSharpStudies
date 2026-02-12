# IX09 - Refactoring Legacy Code: Order Processing System

## Difficulty: Hard
## Estimated Time: 90-120 minutes
## Type: Refactor existing code (write tests first, then refactor)

## Overview

You are given a deliberately messy `OrderProcessor` class that represents a "legacy" order processing system. It has accumulated technical debt over time: deeply nested conditionals, magic numbers, duplicated logic, tight coupling, and thread-safety issues. Your task is to:

1. **Write characterization tests** that document the current behavior (before changing anything).
2. **Refactor** the code into clean, well-structured components.
3. **Verify** that all your characterization tests still pass after refactoring.

This simulates a real-world "legacy rescue" scenario commonly encountered in senior-level interviews.

## Current Code Problems (to identify and fix)

| Problem | Location |
|---------|----------|
| Magic numbers | Discount percentages (0.10, 0.15, 0.20, 0.05), tax rate (0.08), free shipping threshold (50) |
| Deeply nested if/else | Discount calculation based on customer type + order total |
| Duplicated discount logic | Same patterns repeated for different customer types |
| String-based currency parsing | `decimal.Parse(order["Total"])` instead of typed models |
| Tight coupling to Console | `Console.WriteLine` used for logging throughout |
| Static mutable state | `static Dictionary` used as an order cache with no synchronization |
| No separation of concerns | Validation, discount, tax, shipping, logging all in one class |
| No interfaces | Everything is concrete, untestable |
| God method | `ProcessOrder` does everything in 300+ lines |

## Refactoring Goals

After refactoring, the code should have:

| Component | Responsibility |
|-----------|---------------|
| `Order` model | Typed order with properties instead of Dictionary |
| `Customer` model | Typed customer with `CustomerType` enum |
| `IDiscountStrategy` | Discount calculation (Strategy pattern) |
| `RegularCustomerDiscount` | 0% for < $50, 5% for < $100, 10% for >= $100 |
| `PremiumCustomerDiscount` | 10% for < $100, 15% for >= $100, 20% for >= $500 |
| `VIPCustomerDiscount` | 15% flat + extra 5% for orders >= $200 |
| `ITaxCalculator` | Tax calculation |
| `IShippingCalculator` | Shipping cost calculation (free over $50 after discount) |
| `IOrderRepository` | Data access (replaces the static Dictionary) |
| `ILogger` | Logging abstraction |
| `OrderService` | Orchestrator with injected dependencies |

## Step-by-Step Instructions

### Step 1: Write Characterization Tests (30-40 min)
- DO NOT modify `OrderProcessor.cs` yet.
- Create tests in the `.Tests` project that capture current behavior:
  - Regular customer order under $50 (no discount)
  - Regular customer order $50-$99 (5% discount)
  - Regular customer order >= $100 (10% discount)
  - Premium customer order < $100 (10% discount)
  - Premium customer order >= $500 (20% discount)
  - VIP customer (15% + 5% bonus for >= $200)
  - Shipping is free when order total after discount >= $50
  - Shipping costs $5.99 when order total after discount < $50
  - Tax is 8% of post-discount total
  - Invalid order (missing fields) throws

### Step 2: Refactor (50-60 min)
- Extract interfaces and implementations as listed above.
- Keep `OrderProcessor.cs` as `OrderProcessor_Original.cs` for reference.
- Create new files for each component.
- Wire everything up in `Program.cs`.

### Step 3: Verify (10 min)
- All characterization tests still pass against the new `OrderService`.
- You may need to adapt the test setup to use the new interfaces.

## Constraints

- Preserve exact discount/tax/shipping logic during refactoring.
- Tests must exist BEFORE you start refactoring.
- Use xUnit.
- Do NOT delete the original file -- rename it.

## Topics Covered

- Characterization testing (testing legacy code)
- Refactoring techniques (Extract Method, Extract Class, Replace Conditional with Polymorphism)
- Strategy Pattern for discounts
- Dependency Injection
- Thread-safety awareness
- Clean Code principles
