# C# Studies

A comprehensive collection of C# exercises, projects, and interview preparation covering fundamentals through advanced topics like async programming, clean code, microservices, and design patterns.

## Repository Structure

```
CSharpStudies/
├── 01_Fundamentals/                              # Variables, loops, arrays, lists
├── 02_Basics_of_OOP/                              # Classes, properties, constructors
├── 03_OOP_Polymorphism_Inheritance_Interfaces/    # Inheritance, abstract, interfaces
├── 04_Exceptions_and_error_handling/              # try-catch, rethrow, custom exceptions
├── 06_LINQ/                                       # Where, Select, OrderBy, CookieCookbook
├── 07_.Net_under_the_hood/                        # ref modifier, value vs reference types
├── 13_Events/                                     # Delegates, events, event-driven patterns
├── 14_Unit_tests/                                 # xUnit, test organization
├── 15_Clean_Code/                                 # Naming, method refactoring, SOLID
├── 16_Asynchrony_and_Multithreading/              # Threads, Tasks, async/await
├── APICatalogo/                                   # ASP.NET Core API + VShop microservices
└── Interview_Exercises/                           # Senior dev interview preparation
```

---

## Course Modules

### 01 - Fundamentals (Ex01-Ex14 + Assignment)

| Exercise | Topic |
|----------|-------|
| Ex01 | Variables and Operators |
| Ex02 | Boolean Type and Operators |
| Ex03 | If-Else Conditional Statement |
| Ex04 | Methods - AbsoluteOfSum |
| Ex05 | String Interpolation - FormatDate |
| Ex06 | Switch Statement - DescribeDay |
| Ex07 | While Loop - CalculateSumOfNumbersBetween |
| Ex08 | Do-While Loop - RepeatCharacter |
| Ex09 | For Loop - Factorial |
| Ex10 | Arrays - BuildHelloString |
| Ex11 | Arrays - IsWordPresentInCollection |
| Ex12 | Multi-dimensional Arrays - FindMax |
| Ex13 | Foreach Loop - IsAnyWordLongerThan |
| Ex14 | Lists - GetOnlyUpperCaseWords |
| Assignment | TODO List Console App |

### 02 - Basics of OOP (Ex15-Ex21 + Assignment)

| Exercise | Topic |
|----------|-------|
| Ex15 | HotelBooking Class |
| Ex16 | Triangle Class |
| Ex17 | Dog Class (multiple constructors) |
| Ex18 | Properties of the Order Class |
| Ex19 | Computed Properties - DailyAccountState |
| Ex20 | Static Classes - NumberToDayOfWeekTranslator |
| Ex21 | string.Split and string.Join Methods |
| Assignment | Dice Roll Game (with tests) |

### 03 - OOP: Polymorphism, Inheritance & Interfaces (Ex22-Ex27)

| Exercise | Topic |
|----------|-------|
| Ex22 | Inheritance & Overriding - Animals |
| Ex23 | Virtual Methods - StringsProcessor Classes |
| Ex24 | `is` Operator and Null Object |
| Ex25 | Abstract Methods - Shapes |
| Ex26 | Extension Methods - List Extensions |
| Ex27 | Interfaces - Numeric Transformations |

### 04 - Exceptions and Error Handling

| Exercise | Topic |
|----------|-------|
| Try-catch-finally | DivideNumbers |
| Rethrowing_exceptions | Rethrow with `throw` |
| Custom_exception | InvalidTransactionException |

### 06 - LINQ

| Exercise | Topic |
|----------|-------|
| Where_Distinct | Filtering and removing duplicates |
| Select_Average | Projection and aggregation |
| OrderBy_First_Last | Sorting and element access |
| Count_Contains | Counting and membership |
| Any_All | Quantifier operations |
| CookieCookbook | Recipe app (JSON/text, file I/O) |

### 07 - .NET Under the Hood

| Exercise | Topic |
|----------|-------|
| ref_modifier | `ref` parameter passing |

### 13 - Events

| Exercise | Topic |
|----------|-------|
| Events_User_and_BankAccount | Delegates and events |
| Events_WeatherDataAggregator | Event-driven data aggregation |

### 14 - Unit Tests

| Exercise | Topic |
|----------|-------|
| Session14 | Main project under test |
| Session14.Tests | xUnit test suite |

### 15 - Clean Code

| Exercise | Topic |
|----------|-------|
| Ex60 | Naming Refactoring |
| Ex61 | Method Refactoring |
| Ex62 | Method Refactoring |
| Assignment | Password Generator Refactoring (with tests) |

### 16 - Asynchrony and Multithreading (Ex63-Ex67 + Assignment)

| Exercise | Topic |
|----------|-------|
| Ex63 | Creating and Starting New Threads |
| Ex64 | Tasks and Waiting |
| Ex65 | Continuations |
| Ex66 | Handling AggregateException |
| Ex67 | Async/Await |
| Assignment | Quote Finder (async app with tests) |

---

## API & Microservices Projects

### APICatalogo

A simple ASP.NET Core Web API with MySQL and Entity Framework Core for product catalog management.

### VShop (Microservices)

A microservices-based e-commerce platform built with ASP.NET Core:

| Service | Description |
|---------|-------------|
| VShop.ProductApi | Product catalog (Clean Architecture) |
| VShop.CartApi | Shopping cart management |
| VShop.DiscountApi | Coupon and discount system |
| VShop.IdentityServer | OAuth2/OIDC authentication |
| VShop.Web | ASP.NET MVC BFF (Backend for Frontend) |

---

## Interview Preparation

The `Interview_Exercises/` folder contains exercises focused on Senior Software Development Engineer preparation.

### Implementation Exercises (IX01-IX10)

| Exercise | Topic |
|----------|-------|
| IX01 | Advanced LINQ - Query Pipeline |
| IX02 | Generics - Generic Cache |
| IX03 | Design Patterns - Payment Processor |
| IX04 | Error Handling - Retry Policy & Circuit Breaker |
| IX05 | Async/Await - Parallel Data Fetcher |
| IX06 | SOLID Refactoring - Report Generator |
| IX07 | Dependency Injection - Notification Service |
| IX08 | Collections - Priority Task Scheduler |
| IX09 | Refactoring - Legacy Order System |
| IX10 | Minimal API - Product Catalog |

### Job Description Exercises (JD01-JD10)

| Exercise | Topic |
|----------|-------|
| JD01 | xUnit Advanced Testing |
| JD04 | EF Core - Repository Pattern |
| JD05 | Debugging Challenge |
| JD06 | Code Review Exercise |
| JD07 | ASP.NET Core Auth API |
| JD08 | Stored Procedures - Data Access |
| JD09 | Integration Testing Suite |
| JD10 | Order Management System |

Additional resources: `INTERVIEW_CHEATSHEET.md` and `flashcards_csharp_senior.csv`.

---

## How to Use

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download) (or later)

### Run a Specific Exercise

```bash
dotnet run --project 01_Fundamentals/Ex01_Variables_and_Operators
```

### Run Tests

```bash
dotnet test 16_Asynchrony_and_Multithreading/Assignment_Quote_Finder.Tests
```

### Build the Entire Solution

```bash
dotnet build CSharpStudies.sln
```

## License

See [LICENSE.txt](LICENSE.txt).
