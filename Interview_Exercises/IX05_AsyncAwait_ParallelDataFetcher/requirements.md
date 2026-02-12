# IX05 - Async/Await: Parallel Data Fetcher

## Difficulty: Medium
## Estimated Time: 60-90 minutes
## Type: Create from scratch

## Overview

Build a system that queries multiple data sources concurrently, handles timeouts and partial failures gracefully, and demonstrates modern async patterns including `IAsyncEnumerable`. This exercise tests deep understanding of async/await, cancellation, concurrency throttling, and error handling in async code.

## Requirements

### Data Model (provided)

```csharp
public record DataResult(string SourceName, string Data, TimeSpan ResponseTime);
```

### Interface (provided)

```csharp
public interface IDataSource
{
    string Name { get; }
    Task<DataResult> FetchAsync(string query, CancellationToken cancellationToken = default);
}
```

### Fake Data Sources to Implement

Create 3 classes implementing `IDataSource`:

1. **FastDatabaseSource** - Simulates a fast database (delay: 100-300ms). Returns `$"DB result for '{query}'"`.
2. **SlowApiSource** - Simulates a slow REST API (delay: 500-2000ms). Returns `$"API result for '{query}'"`. Has a 20% chance of throwing `HttpRequestException`.
3. **UnreliableCacheSource** - Simulates an unreliable cache (delay: 50-150ms). Has a 30% chance of throwing `TimeoutException`.

Use `Task.Delay` with the simulated delays. Use `Random` for the random failures.

### ParallelDataFetcher

Implement a `ParallelDataFetcher` class:

#### Constructor
- Accepts `IEnumerable<IDataSource> sources` and `int maxConcurrency` (default: 3).

#### Method 1: `FetchAllAsync`
```csharp
Task<IReadOnlyList<DataResult>> FetchAllAsync(string query, TimeSpan timeout)
```
- Queries ALL sources concurrently (respecting `maxConcurrency` via `SemaphoreSlim`).
- Uses a `CancellationTokenSource` with the given `timeout`.
- Returns all **successful** results. Failed or timed-out sources are skipped.
- Should NOT throw if some sources fail -- only if ALL sources fail (throw `AggregateException`).

#### Method 2: `FetchAsResultsArrive`
```csharp
IAsyncEnumerable<DataResult> FetchAsResultsArrive(string query, CancellationToken cancellationToken = default)
```
- Starts all source fetches concurrently.
- Yields each `DataResult` as soon as it completes (fastest first).
- Skips sources that throw exceptions (log to console).
- Respects cancellation.

**Hint**: Use `Task.WhenAny` in a loop or `Channel<DataResult>` from `System.Threading.Channels`.

### Program.cs Demo

The provided `Program.cs` should:
1. Create instances of the 3 data sources.
2. Use `FetchAllAsync` with a 3-second timeout and print all results.
3. Use `FetchAsResultsArrive` and print results as they stream in with `await foreach`.

## Constraints

- Use `SemaphoreSlim` for concurrency throttling in `FetchAllAsync`.
- Use `CancellationToken` properly -- pass it through to data sources.
- Do NOT swallow `OperationCanceledException` silently; handle it intentionally.
- No external libraries required.

## Topics Covered

- async/await
- Task.WhenAll and Task.WhenAny
- IAsyncEnumerable and `await foreach`
- CancellationToken and CancellationTokenSource
- SemaphoreSlim for throttling
- Exception handling in async contexts
- AggregateException
