# Abstract Methods - Shapes

## Description
Make the `GetShapesAreas` method work by implementing an abstract `Shape` class and concrete shape classes.

## Context
The method takes a collection of `Shape` objects and returns their areas as doubles.

## Requirements

### 1. **`Shape`** (abstract base class)
- Abstract method: `CalculateArea()` → `double`

### 2. **`Square`** (concrete class)
- Implements `CalculateArea()`
- Formula: `side × side`

### 3. **`Rectangle`** (concrete class)
- Implements `CalculateArea()`
- Formula: `width × height`

### 4. **`Circle`** (concrete class)
- Implements `CalculateArea()`
- Formula: `π × radius²`
- Use `Math.PI` for π

## Note on Doubles
Floating-point numeric type for decimal values:
```csharp
double someNumber = 10.05;
```

## Topics Covered
- Abstract classes
- Abstract methods
- Polymorphism
- Area calculations
- Math.PI constant