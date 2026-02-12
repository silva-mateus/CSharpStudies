---
name: Job Description Exercises
overview: Create 10 new exercises (JD01-JD10) in a separate directory under `Interview_Exercises/`, focused on SQL querying, schema design, SQL Server/EF Core, advanced xUnit testing, debugging, code review, and ASP.NET Core patterns -- the gaps not covered by the existing IX01-IX10 set.
todos:
  - id: jd-scaffold
    content: Create JD exercise directories and add projects to Interview_Exercises.slnx
    status: completed
  - id: jd01
    content: "JD01 - xUnit Advanced Testing Patterns (Easy): requirements.md + StringCalculator/DateHelper code + empty test skeleton"
    status: completed
  - id: jd02
    content: "JD02 - SQL Querying Fundamentals (Easy): requirements.md + schema-and-seed.sql + empty queries.sql template"
    status: completed
  - id: jd03
    content: "JD03 - Database Schema Design (Medium): requirements.md with business requirements for Library Management System"
    status: completed
  - id: jd04
    content: "JD04 - EF Core Repository Pattern (Medium): requirements.md + starter entities + empty repo/test skeletons"
    status: completed
  - id: jd05
    content: "JD05 - Debugging Challenge (Medium): requirements.md + full buggy codebase + 10 failing tests"
    status: completed
  - id: jd06
    content: "JD06 - Code Review Exercise (Medium): requirements.md + 5 C# files to review + empty review.md template"
    status: completed
  - id: jd07
    content: "JD07 - ASP.NET Core JWT Auth API (Medium): requirements.md + starter project + empty test skeleton"
    status: completed
  - id: jd08
    content: "JD08 - Stored Procedures and Data Access (Hard): requirements.md + schema + starter code + test skeleton"
    status: completed
  - id: jd09
    content: "JD09 - Comprehensive Integration & Acceptance Testing (Hard): requirements.md + full Library API code + empty test skeletons"
    status: completed
  - id: jd10
    content: "JD10 - Full-Stack Order Management Capstone (Hard): requirements.md + starter architecture + test skeleton"
    status: completed
isProject: false
---

# Job Description-Focused Interview Exercises (JD01-JD10)

## Context

The existing IX01-IX10 exercises cover core C# (LINQ, generics, async, design patterns, SOLID, DI, collections, refactoring, and basic Minimal API). The job description emphasizes areas those exercises miss:

- **SQL querying and schema design** (SQL Server)
- **Thorough unit testing and debugging**
- **Integration testing and UAT**
- **Code reviews and quality procedures**
- **ASP.NET Core beyond basic CRUD**

## Directory Structure

New exercises will live alongside the existing ones in `Interview_Exercises/`, using a `JD##` prefix. All projects will be added to the existing `Interview_Exercises.slnx` solution.

```
Interview_Exercises/
  (existing IX01-IX10 folders...)
  JD01_xUnit_Advanced_Testing/
  JD02_SQL_Querying_Fundamentals/
  JD03_Database_Schema_Design/
  JD04_EFCore_Repository_Pattern/
  JD04_EFCore_Repository_Pattern.Tests/
  JD05_Debugging_Challenge/
  JD05_Debugging_Challenge.Tests/
  JD06_Code_Review_Exercise/
  JD07_ASPNetCore_Auth_API/
  JD07_ASPNetCore_Auth_API.Tests/
  JD08_StoredProcs_DataAccess/
  JD08_StoredProcs_DataAccess.Tests/
  JD09_Integration_Testing_Suite/
  JD09_Integration_Testing_Suite.Tests/
  JD10_OrderManagement_System/
  JD10_OrderManagement_System.Tests/
```

## Exercise Breakdown

### EASY (2 exercises, ~30-45 min each)

**JD01 - xUnit Advanced Testing Patterns** (Easy)

- **Type**: Create from scratch
- **Focus**: The "unit tests code thoroughly" requirement
- **Task**: Given a pre-built `StringCalculator` class (add, multiply, parse expressions) and a `DateHelper` utility, write a comprehensive test suite using advanced xUnit features:
  - `[Fact]` and `[Theory]` with `[InlineData]`, `[MemberData]`, `[ClassData]`
  - `IClassFixture<T>` for shared expensive setup
  - `ITestOutputHelper` for diagnostic output
  - Testing exceptions with `Assert.Throws` and FluentAssertions equivalents
  - Test organization: nested classes for grouping, descriptive naming conventions
  - Parameterized edge cases (null, empty, boundary values, culture-specific)
- **Deliverable**: A fully-passing test suite with 20+ test methods covering happy paths, edge cases, and error cases

**JD02 - SQL Querying Fundamentals** (Easy)

- **Type**: Write SQL queries (no C# code)
- **Focus**: "SQL querying" and "relational database access patterns"
- **Setup**: A `.sql` script creates the schema and seeds data in SQL Server LocalDB:
  - `Employees`, `Departments`, `Orders`, `OrderItems`, `Products`, `Customers`
- **Task**: Write 12 SQL queries of increasing complexity:
  1. Basic SELECT with WHERE and ORDER BY
  2. INNER JOIN across two tables
  3. LEFT JOIN to find orphaned records
  4. GROUP BY with HAVING
  5. Subquery in WHERE clause
  6. Correlated subquery
  7. Common Table Expression (CTE)
  8. Window function (ROW_NUMBER, RANK)
  9. Running total with window function (SUM OVER)
  10. PIVOT query
  11. Multi-table JOIN with aggregation (report-style)
  12. UPDATE with JOIN / DELETE with subquery
- **Deliverable**: A `.sql` file with all queries, each with a comment block describing expected results

### MEDIUM (5 exercises, ~60-90 min each)

**JD03 - Database Schema Design** (Medium)

- **Type**: Design exercise (DDL + explanation)
- **Focus**: "Schema design" and "relational database platforms"
- **Task**: Given a business requirements document for a Library Management System (books, authors, members, loans, reservations, fines), design a normalized (3NF) schema:
  - Write CREATE TABLE statements with appropriate data types, constraints (PK, FK, UNIQUE, CHECK, DEFAULT)
  - Add indexes for common query patterns described in the requirements
  - Write seed data INSERT scripts
  - Write 5 complex queries the application would need (e.g., "books currently overdue", "most popular authors this month")
  - Document design decisions in a markdown file (why certain choices were made)
- **Deliverable**: DDL script, seed script, queries script, and design-decisions.md

**JD04 - EF Core Repository with SQL Server** (Medium)

- **Type**: Create from scratch (with integration tests)
- **Focus**: "SQL Server", "C#/.NET", and database access patterns
- **Task**: Build a data access layer for a Blog platform (Posts, Comments, Tags, Authors):
  - Define EF Core entity models with proper relationships (one-to-many, many-to-many)
  - Configure with Fluent API (not data annotations)
  - Implement `IRepository<T>` with generic CRUD + `IBlogRepository` with domain-specific queries (posts by tag, recent posts with comment counts, author statistics)
  - Use SQL Server LocalDB via connection string
  - Write integration tests that use a fresh database per test class (create/drop pattern)
- **Deliverable**: Entity models, DbContext, repositories, and integration tests

**JD05 - Debugging Challenge** (Medium)

- **Type**: Find and fix bugs in existing code
- **Focus**: "Debug thoroughly" and "resolve problems encountered"
- **Task**: A "working" (compiling) order processing system is provided with 10 deliberate bugs:
  1. Off-by-one error in pagination
  2. Race condition in a shared cache
  3. SQL injection vulnerability in a raw query
  4. Async deadlock (`.Result` on an async call)
  5. Null reference from missing null check
  6. Incorrect decimal rounding (banker's rounding vs. standard)
  7. Resource leak (undisposed `HttpClient`)
  8. Incorrect LINQ query (wrong `FirstOrDefault` predicate)
  9. String comparison culture issue (Turkish-I problem)
  10. Exception swallowed silently in a catch block
- **Provided**: Failing test suite (10 tests, one per bug). All tests must pass after fixing.
- **Deliverable**: All 10 bugs fixed, all tests green, and a `bugfix-notes.md` explaining each bug and fix

**JD06 - Code Review Exercise** (Medium)

- **Type**: Written review (no coding)
- **Focus**: "Code reviews, design reviews, team quality procedures"
- **Task**: A "pull request" is provided (5 C# files implementing a UserService with CRUD, auth, and caching). Review the code and write structured feedback identifying:
  - **Bugs** (logic errors, edge cases)
  - **Security issues** (injection, secrets in code, missing auth checks)
  - **Performance problems** (N+1 queries, unnecessary allocations, missing caching)
  - **Design violations** (SOLID, coupling, naming, code smells)
  - **Missing error handling** (unhandled exceptions, missing validation)
  - **Test gaps** (what tests should be written)
- **Format**: A `review.md` with sections for each category, citing specific line numbers and suggesting improvements
- **Deliverable**: A thorough code review document (at least 15 issues identified)

**JD07 - ASP.NET Core API with JWT Auth** (Medium)

- **Type**: Create from scratch (with integration tests)
- **Focus**: "ASP.NET Core", "Web Services", "REST"
- **Task**: Build a Task Management API with JWT authentication:
  - Auth endpoints: `POST /auth/register`, `POST /auth/login` (returns JWT token)
  - Protected endpoints: CRUD for `/tasks` (only the owner can see/edit their tasks)
  - Role-based: Admin can see all tasks, regular users only their own
  - Proper middleware pipeline: authentication, authorization, error handling, request logging
  - Custom `ActionFilter` for model validation
  - API versioning header
- **Integration tests** using `WebApplicationFactory`:
  - Anonymous request returns 401
  - Valid token accesses protected endpoints
  - User cannot access another user's tasks (403)
  - Admin can access all tasks
  - Expired token returns 401
- **Deliverable**: Full API with auth, middleware, and integration tests

### HARD (3 exercises, ~90-120 min each)

**JD08 - Stored Procedures and Data Access Patterns** (Hard)

- **Type**: Create from scratch (with tests)
- **Focus**: "SQL querying", "SQL Server", access patterns
- **Task**: Build a data access layer using multiple approaches for a Product Inventory system:
  - **Part 1**: Write 5 SQL Server stored procedures (CRUD + a complex report with temp tables and cursors)
  - **Part 2**: Implement `IProductRepository` using raw ADO.NET (`SqlConnection`, `SqlCommand`, parameterized queries)
  - **Part 3**: Implement the same `IProductRepository` using Dapper
  - **Part 4**: Write integration tests that work against SQL Server LocalDB, verifying both implementations behave identically
  - Handle: transactions, optimistic concurrency, connection pooling, proper disposal
- **Deliverable**: Stored procedures, ADO.NET repo, Dapper repo, and integration tests

**JD09 - Comprehensive Integration & Acceptance Testing** (Hard)

- **Type**: Write tests for a provided API
- **Focus**: "Integration testing", "user acceptance testing", "resolve problems encountered"
- **Task**: A complete (working) ASP.NET Core API for a Library system is provided (endpoints for books, members, loans). Write a full test pyramid:
  - **Unit tests** (15+): Business logic layer (loan rules, fine calculations, availability checks) using fakes
  - **Integration tests** (10+): API endpoints via `WebApplicationFactory` (CRUD, filters, pagination, error responses)
  - **Database integration tests** (5+): Repository layer against SQL Server LocalDB (verify queries, constraints, cascading deletes)
  - **Acceptance tests** (5+): End-to-end scenarios written as Given/When/Then (e.g., "Given a member borrows a book, When they return it late, Then a fine is calculated")
  - Test helpers: custom `WebApplicationFactory` with test database, API client wrapper, test data builders
- **Deliverable**: 35+ tests across all layers, fully passing

**JD10 - Full-Stack Order Management System** (Hard, Capstone)

- **Type**: Create from scratch (full stack with tests)
- **Focus**: Everything combined -- the capstone exercise
- **Task**: Build a complete Order Management API from requirements:
  - **ASP.NET Core Minimal API** with proper layered architecture (API / Service / Repository)
  - **EF Core + SQL Server LocalDB** with migrations
  - **Schema**: Customers, Products, Orders, OrderLines, Payments (with proper relationships and constraints)
  - **Endpoints**: CRUD for all entities + business endpoints:
    - `POST /orders/{id}/submit` (validates stock, calculates totals)
    - `GET /reports/sales-summary?from=&to=` (aggregated SQL query via EF Core raw SQL or a view)
    - `GET /customers/{id}/order-history` (paged, includes totals)
  - **Validation**: FluentValidation for all inputs
  - **Testing**: Unit tests for service layer, integration tests for API, database tests for complex queries
- **Deliverable**: Complete working system with 20+ tests

## Dependencies

All SQL Server exercises will use **SQL Server LocalDB** (comes with Visual Studio or installable standalone). A shared setup script will be provided to verify LocalDB is available.

NuGet packages needed across exercises:

- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Design` (for migrations)
- `Dapper` (JD08 only)
- `Microsoft.AspNetCore.Authentication.JwtBearer` (JD07)
- `FluentValidation` (JD07, JD10)
- `xunit`, `FluentAssertions`, `Microsoft.NET.Test.Sdk` (all test projects)
- `Microsoft.AspNetCore.Mvc.Testing` (JD07, JD09, JD10)

## File Format

Same conventions as IX01-IX10:

- `requirements.md` with title, overview, requirements tables, constraints, topics covered, estimated time, difficulty
- Starter code with `// TODO` markers
- For "debug" and "code review" exercises: full provided code (buggy or reviewable)
- For "write tests" exercises: full provided application code + empty test skeletons
- `.csproj` files targeting `net9.0`

