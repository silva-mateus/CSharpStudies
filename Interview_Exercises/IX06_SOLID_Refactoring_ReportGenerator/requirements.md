# IX06 - SOLID Refactoring: Report Generator

## Difficulty: Medium
## Estimated Time: 60-90 minutes
## Type: Refactor existing code

## Overview

You are given a `ReportGenerator` class that violates all five SOLID principles. It reads employee data from a hardcoded CSV string, calculates statistics, formats reports in multiple formats, and sends emails -- all in a single monolithic class. Your task is to refactor this code into a clean, extensible architecture while preserving the same output behavior.

## Current Problems (identify these during refactoring)

1. **Single Responsibility Violation**: One class does data reading, statistics, formatting, and email sending.
2. **Open/Closed Violation**: Adding a new report format (e.g., JSON) requires modifying the existing class.
3. **Liskov Substitution Violation**: No base types or abstractions to substitute.
4. **Interface Segregation Violation**: No interfaces at all -- everything is tightly coupled.
5. **Dependency Inversion Violation**: High-level report generation depends on low-level details (CSV parsing, SMTP, Console).

## Refactoring Goals

After refactoring, the code should have:

| Interface | Implementations | Responsibility |
|-----------|----------------|----------------|
| `IDataReader` | `CsvDataReader` | Reading and parsing employee data |
| `IStatisticsCalculator` | `EmployeeStatisticsCalculator` | Calculating min/max/avg/count statistics |
| `IReportFormatter` | `PlainTextReportFormatter`, `HtmlReportFormatter` | Formatting the report output |
| `IReportSender` | `ConsoleReportSender` (replaces email) | Delivering the formatted report |

### Orchestrator: `ReportService`
- Accepts the above interfaces via constructor injection.
- Has a `GenerateAndSendReport()` method that orchestrates the pipeline:
  1. Read data via `IDataReader`
  2. Calculate stats via `IStatisticsCalculator`
  3. Format report via `IReportFormatter`
  4. Send report via `IReportSender`

### Program.cs
- Acts as the composition root, wiring all dependencies.
- Calls `ReportService.GenerateAndSendReport()`.

## Constraints

- The final console output should display a formatted report with the same statistics as the original code.
- Do NOT delete the original `ReportGenerator.cs` -- keep it for reference (rename to `ReportGenerator_Original.cs`).
- Create new files for each interface and implementation.
- You may use LINQ in `IStatisticsCalculator`.

## What NOT to Change

- The CSV data content (keep the same employee records).
- The statistics calculated (count, average salary, min salary, max salary, department breakdown).

## Topics Covered

- SOLID Principles (all five)
- Refactoring techniques
- Interface extraction
- Dependency Injection (manual)
- Separation of concerns
