# JD05 - Debugging Challenge

## Difficulty: Medium
## Estimated Time: 45-60 minutes
## Type: Find and fix bugs in existing code

## Overview

A compiling but buggy order processing system is provided. It contains 10 deliberate bugs representing common real-world defects. A failing test suite is provided with one test per bug. Your goal is to find and fix all 10 bugs, making all tests pass. Document each bug and fix in `bugfix-notes.md`.

## The Bugs (hidden in the code)

You must find these yourself, but here is the category of each bug:

| # | Category | Hint |
|---|----------|------|
| 1 | Off-by-one error | Pagination skips the first item |
| 2 | Race condition | Shared dictionary accessed from multiple threads |
| 3 | SQL injection | String concatenation in a query builder |
| 4 | Async deadlock | `.Result` called on an async method in a sync context |
| 5 | Null reference | Missing null check before accessing a property |
| 6 | Rounding error | Banker's rounding instead of standard (MidpointRounding) |
| 7 | Resource leak | HttpClient created in a loop without disposal |
| 8 | Wrong LINQ predicate | FirstOrDefault uses wrong property for comparison |
| 9 | Culture issue | String comparison fails for Turkish locale |
| 10 | Swallowed exception | Empty catch block hides a critical error |

## Instructions

1. Run the tests: `dotnet test`. All 10 tests will FAIL.
2. For each failing test, read the test name and assertion to understand expected behavior.
3. Find the bug in the source code and fix it.
4. Run tests again until all pass.
5. Document each fix in `bugfix-notes.md`.

## Deliverables

- All 10 bugs fixed (all tests green).
- `bugfix-notes.md` with one entry per bug: what was wrong, where, and how you fixed it.

## Constraints

- Do NOT modify the test files.
- Only fix bugs in the source files under `JD05_Debugging_Challenge/`.
- Each bug has exactly one fix (no redesign needed).

## Topics Covered

- Debugging and problem diagnosis
- Pagination logic
- Thread safety (ConcurrentDictionary)
- SQL injection prevention
- Async/await anti-patterns
- Null safety
- Decimal rounding modes
- IDisposable and resource management
- LINQ correctness
- Culture-aware string comparison
- Exception handling best practices
