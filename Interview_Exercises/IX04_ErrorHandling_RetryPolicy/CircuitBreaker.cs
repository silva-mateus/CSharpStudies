namespace IX04_ErrorHandling_RetryPolicy;

public enum CircuitState
{
    Closed,
    Open,
    HalfOpen
}

public class CircuitBreaker
{
    private readonly int _failureThreshold;
    private readonly TimeSpan _openDuration;
    private readonly IDelayProvider _delayProvider;

    public CircuitState State { get; private set; } = CircuitState.Closed;

    public CircuitBreaker(
        int failureThreshold,
        TimeSpan openDuration,
        IDelayProvider delayProvider)
    {
        _failureThreshold = failureThreshold;
        _openDuration = openDuration;
        _delayProvider = delayProvider;
    }

    /// <summary>
    /// Executes the given async action through the circuit breaker.
    ///
    /// - Closed: Execute normally. On success, reset failures. On failure, increment.
    ///   If failures >= threshold, transition to Open (record the time).
    /// - Open: Throw CircuitBreakerOpenException immediately.
    ///   If openDuration has elapsed since opening, transition to HalfOpen instead.
    /// - HalfOpen: Execute as a trial. On success -> Closed. On failure -> Open.
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        // TODO: your code goes here
        throw new NotImplementedException();
    }
}
