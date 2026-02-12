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

    private int _consecutiveFailures;
    private DateTime? _openedAtUtc;

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

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        if (State == CircuitState.Open)
        {
            if (_delayProvider.UtcNow - _openedAtUtc.GetValueOrDefault() < _openDuration)
                throw new CircuitBreakerOpenException();

            State = CircuitState.HalfOpen; // do a trial run
        }

        try
        {
            var result = await action();

            _consecutiveFailures = 0;
            State = CircuitState.Closed;
            return result;
        }
        catch
        {
            if (State == CircuitState.HalfOpen)
            {
                State = CircuitState.Open;
                _openedAtUtc = _delayProvider.UtcNow;
                throw;
            }

            //Closed
            _consecutiveFailures++;
            if (_consecutiveFailures >= _failureThreshold)
            {
                State = CircuitState.Open;
                _openedAtUtc = _delayProvider.UtcNow;
            }
            throw;
        }
    }
}
