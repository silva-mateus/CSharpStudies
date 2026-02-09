# Triangle Class

## Description
Implement the `Triangle` class with private fields and public methods.

## Requirements

### Private Fields
- `base` (int) - first constructor parameter
- `height` (int) - second constructor parameter

### Public Methods
1. **`CalculateArea()`**
   - Returns: `(base × height) / 2`
   - Note: Result is truncated to integer

2. **`AsString()`**
   - Returns: `"Base is B, height is H"` (with actual values)
   - Example: `"Base is 10, height is 5"`

## Important Notes
- `base` is a reserved keyword in C# - use `@base` in the constructor parameter
- Area calculation truncates decimals (e.g., 1.5 becomes 1)

## Topics Covered
- Private fields
- Public methods
- Reserved keywords (`@` prefix)
- Integer division