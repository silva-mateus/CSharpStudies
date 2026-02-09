# Lists - GetOnlyUpperCaseWords

## Description
Implement the `GetOnlyUpperCaseWords` method that filters a collection of strings to return only those containing exclusively uppercase letters.

## Requirements
- Return only words with **all uppercase letters**
- Remove duplicates from the result
- Digits and special characters disqualify a word

## Examples
- `{"one", "TWO", "THREE", "four"}` → `{"TWO", "THREE"}`
- `{"one", "TWO", "THREE", "four", "TWO"}` → `{"TWO", "THREE"}` (duplicate removed)
- `{"one", "TWO123", "THREE!&^", "four"}` → `{}` (empty - contains non-letters)

## Tips
Strings are collections of characters, so you can iterate them with `foreach` to check each character.

## Topics Covered
- Lists
- String iteration
- Character validation (`char.IsUpper()`, `char.IsLetter()`)
- Duplicate removal
- LINQ or manual filtering