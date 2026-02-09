# "is" Operator and Null Object

## Description
Implement the `Describe` method that identifies numeric types and returns their descriptions, or `null` for other types.

## Requirements

### Method Signature
```csharp
public string Describe(object input)
```

### Behavior by Type
- **`int`** → `"Int of value {value}"`
- **`double`** → `"Double of value {value}"`
- **`decimal`** → `"Decimal of value {value}"`
- **Any other type** → `null`

## Examples
- `Describe(5)` → `"Int of value 5"`
- `Describe(5.6)` → `"Double of value 5.6"`
- `Describe(5.7m)` → `"Decimal of value 5.7"`
- `Describe("text")` → `null`

## Topics Covered
- `is` operator
- Type checking
- Null return values
- Object parameter
- Numeric types (int, double, decimal)