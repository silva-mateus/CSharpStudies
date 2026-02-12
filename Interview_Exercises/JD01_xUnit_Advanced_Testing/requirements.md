# JD01 - xUnit Advanced Testing Patterns

## Difficulty: Easy
## Estimated Time: 30-45 minutes
## Type: Create tests from scratch for provided code

## Overview

You are given two fully-implemented utility classes: `StringCalculator` and `DateHelper`. Your task is to write a comprehensive test suite demonstrating mastery of xUnit features and testing best practices. This exercise focuses on the "unit tests code thoroughly" aspect of the role.

## Provided Code (do not modify)

### StringCalculator
A calculator that parses and evaluates simple arithmetic expressions from strings.

| Method | Signature | Description |
|--------|-----------|-------------|
| Add | `int Add(string numbers)` | Adds comma- or newline-separated numbers. Empty string returns 0. Negative numbers throw `ArgumentException`. |
| Multiply | `int Multiply(string numbers)` | Same as Add but multiplies. Empty returns 1. |
| ParseExpression | `double ParseExpression(string expr)` | Evaluates "a op b" (e.g., "3 + 4"). Supports +, -, *, /. Division by zero throws `DivideByZeroException`. |
| GetCallerCount | `int GetCallerCount()` | Returns how many times Add has been called since last reset. |
| Reset | `void Reset()` | Resets the caller count. |

### DateHelper
A utility for date calculations.

| Method | Signature | Description |
|--------|-----------|-------------|
| GetBusinessDaysBetween | `int GetBusinessDaysBetween(DateTime start, DateTime end)` | Counts weekdays between two dates (exclusive of end). Throws if start > end. |
| IsLeapYear | `bool IsLeapYear(int year)` | Standard leap year rules. Throws if year <= 0. |
| GetQuarter | `int GetQuarter(DateTime date)` | Returns 1-4 for Q1-Q4. |
| FormatRelative | `string FormatRelative(DateTime date, DateTime now)` | Returns "today", "yesterday", "X days ago", "in X days", etc. |

## Tests to Write

Write tests in `JD01_xUnit_Advanced_Testing.Tests` demonstrating ALL of the following xUnit features:

### Required xUnit Features (must use each at least once)

| Feature | Usage |
|---------|-------|
| `[Fact]` | Basic test methods |
| `[Theory]` + `[InlineData]` | Parameterized tests with inline values |
| `[Theory]` + `[MemberData]` | Parameterized tests with method/property data source |
| `[Theory]` + `[ClassData]` | Parameterized tests with a class data source |
| `IClassFixture<T>` | Shared setup across all tests in a class |
| `ITestOutputHelper` | Diagnostic output during tests |
| `Assert.Throws<T>` | Exception testing |
| FluentAssertions | Use `.Should().Be()`, `.Should().Throw()`, etc. |
| Nested classes | Group related tests inside a parent test class |

### Test Coverage Requirements (minimum 20 test methods)

**StringCalculator (12+ tests):**
1. Add with empty string returns 0
2. Add with single number returns that number
3. Add with two comma-separated numbers
4. Add with newline-separated numbers
5. Add with negative number throws ArgumentException (with message check)
6. Add with multiple numbers (use `[Theory]` + `[InlineData]`)
7. Multiply with empty string returns 1
8. Multiply with various inputs (use `[Theory]` + `[MemberData]`)
9. ParseExpression with all four operators (use `[Theory]` + `[ClassData]`)
10. ParseExpression with division by zero throws
11. ParseExpression with invalid format throws
12. GetCallerCount increments and resets correctly (use `IClassFixture`)

**DateHelper (8+ tests):**
13. GetBusinessDaysBetween for a normal week
14. GetBusinessDaysBetween spanning a weekend
15. GetBusinessDaysBetween with start > end throws
16. IsLeapYear with various years (use `[Theory]` + `[InlineData]`)
17. IsLeapYear with invalid year throws
18. GetQuarter for each quarter (use `[Theory]`)
19. FormatRelative for "today", "yesterday", "X days ago"
20. FormatRelative for future dates

## Constraints

- Do NOT modify the provided `StringCalculator` or `DateHelper` classes.
- Use both `Assert` (xUnit built-in) and `FluentAssertions` syntax.
- Test class must use `ITestOutputHelper` to log at least one diagnostic message.
- Organize `StringCalculator` tests with nested classes (e.g., `AddTests`, `MultiplyTests`, `ParseExpressionTests`).

## Topics Covered

- xUnit: `[Fact]`, `[Theory]`, `[InlineData]`, `[MemberData]`, `[ClassData]`
- `IClassFixture<T>` for shared test state
- `ITestOutputHelper` for diagnostics
- FluentAssertions
- Exception testing patterns
- Test organization and naming conventions
- Boundary value testing
