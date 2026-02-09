# Computed Properties - DailyAccountState Class

## Description
Complete the `DailyAccountState` class by adding computed properties that derive values from existing properties.

## Existing Properties
- `InitialState` - Money in account at the start of the day
- `SumOfOperations` - Total of incomes and outcomes for the day

## Example
Initial state: $2000, Sum of operations: -$200 → End of day: $1800

## Properties to Add

### 1. `EndOfDayState` (computed)
Returns: `InitialState + SumOfOperations`

### 2. `Report` (computed)
Returns string in format:
```
"Day: 15, month: 3, year: 2025, initial state: 2000, end of day state: 1800"
```
- Day, month, year: from current date
- States: from object's properties

## Topics Covered
- Computed properties
- Read-only properties
- DateTime properties (Day, Month, Year)
- String formatting