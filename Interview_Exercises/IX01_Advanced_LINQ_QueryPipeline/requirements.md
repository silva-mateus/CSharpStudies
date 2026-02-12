# IX01 - Advanced LINQ: Query Pipeline

## Difficulty: Easy
## Estimated Time: 30-45 minutes
## Type: Create from scratch

## Overview

You are given a list of `Employee` records. Your task is to implement a `EmployeeQueryService` class that provides several analytical methods using LINQ. This exercise tests your fluency with LINQ method syntax, grouping, aggregation, and projection.

## Requirements

### Data Model

An `Employee` record is already provided with the following properties:

| Property     | Type       | Description                  |
|--------------|------------|------------------------------|
| Name         | string     | Full name of the employee    |
| Department   | string     | Department name              |
| Salary       | decimal    | Annual salary                |
| HireDate     | DateTime   | Date the employee was hired  |

### Methods to Implement

Implement the following methods in `EmployeeQueryService`:

#### 1. `GetAverageSalaryByDepartment()`
- Returns an `IEnumerable<DepartmentSalary>` with `Department` (string) and `AverageSalary` (decimal).
- Results should be ordered by department name ascending.

#### 2. `GetTopEarners(int count)`
- Returns the top `count` employees by salary, ordered descending.
- If two employees have the same salary, order by name ascending.
- Returns `IEnumerable<Employee>`.

#### 3. `GetRecentHighEarners(int yearsBack, decimal salaryThreshold)`
- Returns employees hired within the last `yearsBack` years (from `DateTime.Now`) whose salary is above `salaryThreshold`.
- Ordered by hire date descending (most recent first).
- Returns `IEnumerable<Employee>`.

#### 4. `GetDepartmentSummaries()`
- Returns an `IEnumerable<DepartmentSummary>` with:
  - `Department` (string)
  - `EmployeeCount` (int)
  - `MinSalary` (decimal)
  - `MaxSalary` (decimal)
  - `AverageSalary` (decimal)
- Ordered by `EmployeeCount` descending, then by department name ascending.

#### 5. `GetDepartmentsAboveBudget(decimal budgetPerEmployee)`
- Returns department names where the average salary exceeds `budgetPerEmployee`.
- Returns `IEnumerable<string>`, ordered alphabetically.

## Constraints

- Use LINQ method syntax (not query syntax).
- Do not use explicit `for` / `foreach` loops for the query logic.
- You may modify only the `EmployeeQueryService.cs` file.

## Topics Covered

- LINQ (`GroupBy`, `Select`, `Where`, `OrderBy`, `ThenBy`, `Take`, `Average`, `Min`, `Max`)
- Records
- Method chaining
- Projection into anonymous types or DTOs
