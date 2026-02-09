# Virtual Methods - StringsProcessor Classes

## Description
Implement a class hierarchy for processing collections of strings with different transformations.

## Goal
Make the `ProcessAll` method work by implementing three classes that process strings.

## How ProcessAll Works
Takes a List of strings and applies transformations using a collection of `StringsProcessor` objects.

## Classes to Implement

### 1. **`StringsProcessor`** (base class)
- Method: `Process(List<string>)` → `List<string>`
- Base implementation (may return input unchanged or define as virtual)

### 2. **`StringsTrimmingProcessor`** (derived)
- Trims each word to half its length
- Example: `["bobcat", "wolverine", "grizzly"]` → `["bob", "wolv", "gri"]`
- Use `Substring(0, word.Length / 2)`

### 3. **`StringsUppercaseProcessor`** (derived)
- Converts each word to uppercase
- Example: `["bobcat", "wolverine", "grizzly"]` → `["BOBCAT", "WOLVERINE", "GRIZZLY"]`

## Combined Result
Input: `["bobcat", "wolverine", "grizzly"]`  
Output: `["BOB", "WOLV", "GRI"]` (trimmed AND uppercase)

## Topics Covered
- Virtual methods
- Method overriding
- Inheritance
- Avoiding code duplication
- Collection processing