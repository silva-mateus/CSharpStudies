---
name: CSharp Interview Exercises
overview: Create a new directory `Interview_Exercises` containing 10 self-contained C# exercises (Easy/Medium/Hard) covering async, design patterns, SOLID, LINQ, generics, DI, collections, error handling, refactoring, and REST API design. Each exercise includes a requirements markdown file, starter code, and some require writing unit tests.
todos:
  - id: scaffold
    content: Create Interview_Exercises/ directory and Interview_Exercises.sln solution file
    status: completed
  - id: ix01
    content: "IX01 - Advanced LINQ: Query Pipeline (Easy) - requirements.md + starter code"
    status: completed
  - id: ix02
    content: "IX02 - Generics: Generic Cache (Easy, with tests) - requirements.md + starter code + test project skeleton"
    status: completed
  - id: ix03
    content: "IX03 - Design Patterns: Payment Processor (Medium, with tests) - requirements.md + starter code + test project"
    status: completed
  - id: ix04
    content: "IX04 - Error Handling: Retry Policy with Circuit Breaker (Medium, with tests) - requirements.md + starter code + test project"
    status: completed
  - id: ix05
    content: "IX05 - Async/Await: Parallel Data Fetcher (Medium) - requirements.md + starter code"
    status: completed
  - id: ix06
    content: "IX06 - SOLID Refactoring: Report Generator (Medium) - requirements.md + full messy code to refactor"
    status: completed
  - id: ix07
    content: "IX07 - DI and Testability: Notification Service (Medium, with tests) - requirements.md + starter code + test project"
    status: completed
  - id: ix08
    content: "IX08 - Collections: Priority Task Scheduler (Hard, with tests) - requirements.md + starter code + test project"
    status: completed
  - id: ix09
    content: "IX09 - Refactoring Legacy Code: Order Processing System (Hard, with tests) - requirements.md + full messy code"
    status: completed
  - id: ix10
    content: "IX10 - Minimal API: Product Catalog (Hard, with tests) - requirements.md + starter code + test project"
    status: completed
isProject: false
---

# C# Senior Interview Exercises Plan

## Directory Structure

A new top-level folder `Interview_Exercises/` will be created alongside the existing topic folders. Each exercise gets its own subfolder following the existing naming convention (`IX##_Description`). Exercises that require tests will include a `.Tests` project. A dedicated solution file `Interview_Exercises.sln` will tie everything together.

```
Interview_Exercises/
├── Interview_Exercises.sln
├── IX01_Advanced_LINQ_QueryPipeline/
├── IX02_Generics_GenericCache/
├── IX03_DesignPatterns_PaymentProcessor/
├── IX04_ErrorHandling_RetryPolicy/
├── IX05_AsyncAwait_ParallelDataFetcher/
├── IX06_SOLID_Refactoring_ReportGenerator/
├── IX07_DI_NotificationService/
├── IX08_Collections_PriorityTaskScheduler/
├── IX09_Refactoring_LegacyOrderSystem/
└── IX10_MinimalAPI_ProductCatalog/
```

## Exercise Breakdown (10 exercises, 3 difficulty tiers)

### EASY (2 exercises, ~30-45 min each)

**IX01 - Advanced LINQ: Query Pipeline** (Easy)

- **Type**: Create from scratch
- **Tests required**: No
- **Task**: Given a list of `Employee` records (name, department, salary, hire date), implement a `ReportGenerator` class with methods that use LINQ to: (1) group employees by department and return average salary per department, (2) find the top N earners across all departments, (3) find employees hired in the last N years with salary above a threshold, (4) return a "department summary" with min/max/avg salary and headcount. Focuses on `GroupBy`, `Select`, `Where`, `OrderBy`, `Aggregate`, and method chaining.

**IX02 - Generics: Generic Cache** (Easy)

- **Type**: Create from scratch
- **Tests required**: Yes (write unit tests)
- **Task**: Implement a `GenericCache<TKey, TValue>` class with: `Add(key, value, TimeSpan expiry)`, `TryGet(key, out value)` returning false if expired, `Remove(key)`, `Count` (only non-expired entries), and `Cleanup()` to purge expired entries. Must use generic constraints (`where TKey : notnull`). Tests should cover expiration, missing keys, duplicate keys, and cleanup behavior.

### MEDIUM (5 exercises, ~60-90 min each)

**IX03 - Design Patterns: Payment Processor** (Medium)

- **Type**: Create from scratch
- **Tests required**: Yes (write unit tests)
- **Task**: Build a payment processing system using the **Strategy** and **Factory** patterns. Define an `IPaymentStrategy` interface with `ProcessPayment(PaymentRequest)` returning `PaymentResult`. Implement strategies for `CreditCard`, `BankTransfer`, and `Cryptocurrency`. Create a `PaymentStrategyFactory` that resolves the correct strategy by `PaymentMethod` enum. Add validation rules per strategy (e.g., credit card number format, IBAN format). Write unit tests verifying each strategy and the factory. Bonus: add a `CompositeValidator` using the **Composite** pattern.

**IX04 - Error Handling: Retry Policy with Circuit Breaker** (Medium)

- **Type**: Create from scratch
- **Tests required**: Yes (write unit tests)
- **Task**: Implement a `RetryPolicy<T>` class that executes a `Func<Task<T>>` with configurable max retries, delay between retries (with exponential backoff), and a list of retriable exception types. Then implement a `CircuitBreaker<T>` wrapper with three states (Closed, Open, HalfOpen). When failures exceed a threshold, the circuit opens and immediately throws for a configured duration. After the timeout, it allows one trial call (HalfOpen). Tests should verify retry counts, backoff timing (using a fake delay), circuit state transitions, and exception propagation for non-retriable exceptions.

**IX05 - Async/Await: Parallel Data Fetcher** (Medium)

- **Type**: Create from scratch
- **Tests required**: No
- **Task**: Build an `IDataSource` interface with `Task<DataResult> FetchAsync(string query, CancellationToken ct)`. Implement 3 fake data sources with artificial delays. Create a `ParallelDataFetcher` that: (1) queries all sources concurrently, (2) supports a configurable timeout via `CancellationToken`, (3) returns results as they complete using `Task.WhenAll` and also a version using `IAsyncEnumerable<DataResult>` that yields results as they arrive, (4) handles partial failures gracefully (returns successful results even if some sources fail). Demonstrate `SemaphoreSlim` for throttling concurrent requests.

**IX06 - SOLID Refactoring: Report Generator** (Medium)

- **Type**: Refactor existing (messy) code
- **Tests required**: No (but code must remain functional)
- **Task**: A `ReportGenerator` class is provided that violates all SOLID principles -- it reads data from a hardcoded CSV string, formats it, calculates statistics, and sends email, all in a single 200+ line class. The candidate must refactor it into separate responsibilities: `IDataReader`, `IReportFormatter` (with `HtmlReportFormatter` and `PlainTextReportFormatter`), `IStatisticsCalculator`, and `IReportSender`. The refactored code must preserve the same public behavior. A `Program.cs` acts as the composition root.

**IX07 - DI and Testability: Notification Service** (Medium)

- **Type**: Create from scratch
- **Tests required**: Yes (write unit tests with fakes/mocks)
- **Task**: Design a notification system: `INotificationChannel` (Email, SMS, Push) each with `SendAsync(Notification)`. Create a `NotificationService` that accepts `IEnumerable<INotificationChannel>` via constructor injection and dispatches to the appropriate channel(s) based on user preferences (`NotificationPreference`). Include an `IUserPreferenceRepository` for retrieving preferences. Write unit tests using hand-rolled fakes (no mocking library required) to verify: correct channel dispatch, multi-channel delivery, graceful handling when a channel fails (other channels still notified), and logging of failures via `ILogger` interface.

### HARD (3 exercises, ~90-120 min each)

**IX08 - Collections: Priority Task Scheduler** (Hard)

- **Type**: Create from scratch
- **Tests required**: Yes (write unit tests)
- **Task**: Implement a `PriorityTaskScheduler<T>` where `T : IScheduledTask`. `IScheduledTask` has `Priority` (enum: Critical, High, Normal, Low), `DueDate`, `Execute()`, and `Id`. The scheduler must: (1) use a custom min-heap internally sorted by priority then due date, (2) support `Enqueue`, `Dequeue`, `Peek`, `Count`, (3) implement `IEnumerable<T>`, (4) be thread-safe using `ReaderWriterLockSlim`, (5) support `ExecuteNext()` and `ExecuteAll()` with a configurable max-parallelism. Write thorough unit tests including concurrent enqueue/dequeue from multiple threads.

**IX09 - Refactoring Legacy Code: Order Processing System** (Hard)

- **Type**: Refactor existing (messy) code
- **Tests required**: Yes (write tests BEFORE refactoring, then refactor)
- **Task**: A deliberately messy `OrderProcessor` class (~300 lines) is provided with: nested if/else blocks, magic numbers, string parsing for currency, tight coupling to `Console.WriteLine` for logging, duplicated discount logic, no separation of concerns, and race conditions in a static dictionary cache. The candidate must: (1) write characterization tests that capture current behavior, (2) incrementally refactor to clean code using patterns (Strategy for discount, Repository for data access, proper DI), (3) ensure all tests still pass after refactoring. This simulates a real-world "legacy rescue" scenario.

**IX10 - Minimal API: Product Catalog** (Hard)

- **Type**: Create from scratch
- **Tests required**: Yes (integration tests with `WebApplicationFactory`)
- **Task**: Build a .NET Minimal API for a product catalog: `GET /products` (with filtering by category, price range, and paging), `GET /products/{id}`, `POST /products`, `PUT /products/{id}`, `DELETE /products/{id}`. Use an in-memory repository (no database required). Include: proper DTO/model separation, `FluentValidation` for input validation, global error handling middleware, response pagination model, and `Results.Problem()` for error responses. Write integration tests using `WebApplicationFactory<Program>` to test all endpoints including validation failures and 404 scenarios.

## File Format for Each Exercise

Following the existing conventions in the repo:

- **Requirements file**: `requirements.md` with title, overview, detailed requirements (tables for scenarios where appropriate), constraints, topics covered, estimated time, and difficulty level.
- **Starter code**: `Program.cs` and relevant `.cs` files. For "refactor" exercises, provide the full messy codebase. For "create" exercises, provide skeleton interfaces/classes with `// TODO` or `// your code goes here` comments where appropriate, plus the `Program.cs` entry point with sample usage.
- **Project file**: `.csproj` targeting `net9.0` with `ImplicitUsings` and `Nullable` enabled.
- **Test projects** (where required): `ExerciseName.Tests/` with `xunit` + `FluentAssertions` dependencies. For exercises where writing tests IS the task, provide only the test project skeleton.

## Solution File

`Interview_Exercises.sln` will include all 10+ projects (exercises + their test projects) for easy one-click build/run.

## Implementation Order

Exercises will be created in order (IX01 through IX10). Each exercise is self-contained, so they can be tackled in any order by the user.