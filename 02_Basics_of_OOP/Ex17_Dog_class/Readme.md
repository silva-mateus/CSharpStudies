# Dog Class

## Description
Define a `Dog` class with multiple constructors and a description method.

## Requirements

### Fields
- `name` (string)
- `breed` (string)
- `weight` (int) - in kilograms

### Constructors
1. **Three-parameter constructor**: `name`, `breed`, `weight` (in order)
2. **Two-parameter constructor**: `name`, `weight` (in order)
   - Sets `breed` to `"mixed-breed"`

### Public Method
**`Describe()`** - Returns a formatted string:
```
"This dog is named {name}, it's a {breed}, and it weighs {weight} kilograms, so it's a {size} dog."
```

### Weight-to-Size Mapping
- `< 5 kg` → `"tiny"`
- `5-29 kg` → `"medium"`
- `≥ 30 kg` → `"large"`

## Examples
- Lucky, german shepherd, 40 kg → `"...it's a large dog."`
- Tina, shar pei, 25 kg → `"...it's a medium dog."`

## Topics Covered
- Multiple constructors
- Constructor chaining
- String interpolation
- Conditional logic