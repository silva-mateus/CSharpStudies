using IX04_ErrorHandling_RetryPolicy;

namespace IX04_ErrorHandling_RetryPolicy.Tests;

/// <summary>
/// TODO: Write unit tests for RetryPolicy and CircuitBreaker.
/// Use FakeDelayProvider to control time and verify delays.
///
/// RetryPolicy tests:
///   1. Succeeds on first try - no retries, no delays.
///   2. Fails twice then succeeds on third attempt.
///   3. Exhausts all retries and throws the last exception.
///   4. Non-retriable exception is thrown immediately without retry.
///   5. Verify exponential backoff delays via FakeDelayProvider.RecordedDelays.
///
/// CircuitBreaker tests:
///   6. Stays Closed when actions succeed.
///   7. Opens after consecutive failures reach the threshold.
///   8. Throws CircuitBreakerOpenException when Open.
///   9. Transitions to HalfOpen after openDuration elapses (use FakeDelayProvider.Advance).
///  10. Successful call in HalfOpen transitions back to Closed.
///  11. Failed call in HalfOpen transitions back to Open.
/// </summary>
public class RetryPolicyTests
{
    // TODO: your tests go here
}

public class CircuitBreakerTests
{
    // TODO: your tests go here
}
