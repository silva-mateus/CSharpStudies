# Properties of the Order Class

## Description
Complete the implementation of the `Order` class by adding two properties with specific getter/setter behaviors.

## Requirements

### Properties

1. **`Item` (string)**
   - Get: Yes
   - Set: **No** (read-only)

2. **`Date` (DateTime)**
   - Get: Yes
   - Set: Yes, **with validation**
     - Only allows setting if the year matches the current year
     - If validation fails, the property value doesn't change

## Topics Covered
- Properties (get/set)
- Read-only properties
- Property validation
- Backing fields
- DateTime comparison