# IX02 - Generics: Generic Cache

## Difficulty: Easy
## Estimated Time: 30-45 minutes
## Type: Create from scratch (with unit tests)

## Overview

Implement a generic, expiration-aware, in-memory cache. This exercise tests your understanding of generics, generic constraints, time-based logic, and writing unit tests.

## Requirements

### Class to Implement: `GenericCache<TKey, TValue>`

**Generic Constraints**: `TKey : notnull`

#### Methods / Properties

| Member | Signature | Description |
|--------|-----------|-------------|
| Add | `void Add(TKey key, TValue value, TimeSpan expiry)` | Adds an item with an expiration duration. If the key already exists, throws `ArgumentException`. |
| TryGet | `bool TryGet(TKey key, out TValue? value)` | Returns `true` and sets `value` if the key exists and is NOT expired. Returns `false` otherwise. |
| Remove | `bool Remove(TKey key)` | Removes the entry. Returns `true` if it existed, `false` otherwise. |
| Count | `int Count { get; }` | Returns the number of non-expired entries. |
| Cleanup | `void Cleanup()` | Removes all expired entries from the internal storage. |

### Behavior Details

- When `TryGet` is called for an expired key, it should return `false` and remove the expired entry.
- `Count` should not count expired entries (but does not need to remove them).
- `Add` with a duplicate key should throw even if the existing entry is expired. Call `Cleanup()` first if you want to reuse keys.

### Design Hint

Consider using an `ITimeProvider` interface (with a `DateTime UtcNow` property) injected into the cache, so that tests can control time without relying on `Thread.Sleep`. A default `SystemTimeProvider` should be used when none is provided.

## Unit Tests to Write

Create tests in `IX02_Generics_GenericCache.Tests` that cover:

1. Adding and retrieving a value before expiry.
2. `TryGet` returns `false` after expiry.
3. `Add` with duplicate key throws `ArgumentException`.
4. `Remove` returns `true` for existing key, `false` for missing key.
5. `Count` does not include expired entries.
6. `Cleanup` removes expired entries so keys can be reused.
7. Cache works with different key/value types (e.g., `int`/`string`, `string`/`object`).

## Constraints

- You must use generic constraints.
- Do NOT use `Thread.Sleep` in tests -- use the `ITimeProvider` approach.
- Use xUnit as the test framework.

## Topics Covered

- Generics and generic constraints
- Dictionary / internal storage design
- Time-based expiration
- Interface-based testability (Dependency Inversion)
- Unit testing with xUnit
