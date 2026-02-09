# Inheritance & Overriding - Animals

## Description
Define animal classes using inheritance and method overriding to represent different animals with varying leg counts.

## Context
The `GetCountsOfAnimalsLegs` method iterates a List of Animals (Lion, Tiger, Duck, Spider) and collects their leg counts.

## Requirements

### Classes to Define
1. **`Animal`** (base class)
   - Property: `NumberOfLegs` (default: 4)

2. **`Lion`** (inherits from Animal)
   - Legs: 4 (use default)

3. **`Tiger`** (inherits from Animal)
   - Legs: 4 (use default)

4. **`Duck`** (inherits from Animal)
   - Legs: 2 (override)

5. **`Spider`** (inherits from Animal)
   - Legs: 8 (override)

## Expected Result
The method should return: `[4, 4, 2, 8]`

## Topics Covered
- Inheritance
- Base classes
- Method/property overriding
- Virtual and override keywords