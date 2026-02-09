# string.Split and string.Join Methods

## Introduction
Learn to use `string.Split` and `string.Join` methods for string manipulation.

### `string.Split`
Splits a string into an array using a separator (char or string).

**Example:**
```csharp
"this,is,some,string".Split(',')
// Returns: ["this", "is", "some", "string"]
```

### `string.Join`
Joins a collection of strings with a separator (static method).

**Example:**
```csharp
string.Join("--", new[] {"one", "two", "three"})
// Returns: "one--two--three"
```

## Exercise

### Task
Implement the `TransformSeparators` method that replaces one separator with another.

### Example
- **Input:** `"this,is,some,string"`
- **Original separator:** `","`
- **Target separator:** `"+"`
- **Result:** `"this+is+some+string"`

## Topics Covered
- `string.Split()`
- `string.Join()`
- String manipulation
- Method chaining