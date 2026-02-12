# IX04 - Error Handling: Retry Policy with Circuit Breaker

## Difficulty: Medium
## Estimated Time: 60-90 minutes
## Type: Create from scratch (with unit tests)

## Overview

Implement two resilience patterns commonly used in distributed systems: a **Retry Policy** with exponential backoff and a **Circuit Breaker**. This tests your understanding of exception handling, async programming, and state machine design.

## Requirements

### Part 1: RetryPolicy

#### Class: `RetryPolicy`

**Constructor Parameters**:
- `int maxRetries` - Maximum number of retry attempts (e.g., 3 means up to 4 total calls).
- `TimeSpan initialDelay` - The delay before the first retry.
- `IDelayProvider delayProvider` - Abstraction for `Task.Delay` (for testability).
- `params Type[] retriableExceptions` - Exception types that should trigger a retry. If empty, retry on all exceptions.

**Method**: `Task<T> ExecuteAsync<T>(Func<Task<T>> action)`
- Executes the action.
- If it throws a retriable exception, waits and retries.
- Delay doubles on each retry (exponential backoff): `initialDelay`, `initialDelay * 2`, `initialDelay * 4`, ...
- If all retries are exhausted, throws the last exception.
- If a non-retriable exception is thrown, rethrow immediately without retrying.

### Part 2: CircuitBreaker

#### Enum: `CircuitState` - `Closed`, `Open`, `HalfOpen`

#### Class: `CircuitBreaker`

**Constructor Parameters**:
- `int failureThreshold` - Number of consecutive failures before opening the circuit.
- `TimeSpan openDuration` - How long the circuit stays open before transitioning to half-open.
- `IDelayProvider delayProvider` - For testable time tracking.

**Method**: `Task<T> ExecuteAsync<T>(Func<Task<T>> action)`
- **Closed** (normal): Execute the action. On success, reset failure count. On failure, increment count. If count >= threshold, transition to Open.
- **Open**: Immediately throw `CircuitBreakerOpenException` without executing the action. After `openDuration` has elapsed, transition to HalfOpen.
- **HalfOpen**: Execute the action as a trial. On success, transition to Closed and reset. On failure, transition back to Open.

**Property**: `CircuitState State { get; }` - Current state of the circuit.

### Supporting Types (provided)

- `IDelayProvider` interface with `Task DelayAsync(TimeSpan delay)` and `DateTime UtcNow { get; }`.
- `CircuitBreakerOpenException` custom exception.

## Unit Tests to Write

Create tests that cover:

1. **RetryPolicy**: Succeeds on first try, no retries needed.
2. **RetryPolicy**: Fails twice then succeeds - verify 2 retries occurred.
3. **RetryPolicy**: Exhausts all retries and throws the final exception.
4. **RetryPolicy**: Non-retriable exception is thrown immediately (no retry).
5. **RetryPolicy**: Verify exponential backoff delays (inspect calls to `IDelayProvider`).
6. **CircuitBreaker**: Stays Closed on success.
7. **CircuitBreaker**: Opens after reaching failure threshold.
8. **CircuitBreaker**: Throws `CircuitBreakerOpenException` when Open.
9. **CircuitBreaker**: Transitions to HalfOpen after open duration elapses.
10. **CircuitBreaker**: Successful trial in HalfOpen transitions to Closed.
11. **CircuitBreaker**: Failed trial in HalfOpen transitions back to Open.

## Constraints

- Do NOT use `Thread.Sleep` or real `Task.Delay` in tests -- use `IDelayProvider`.
- The circuit breaker should be safe for single-threaded use (thread-safety is a bonus).
- Use xUnit as the test framework.

## Topics Covered

- Exception handling and custom exceptions
- Async/await patterns
- State machine design (Circuit Breaker)
- Exponential backoff
- Interface-based testability
- Unit testing async code
