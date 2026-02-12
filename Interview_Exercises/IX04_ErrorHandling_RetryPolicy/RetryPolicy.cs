namespace IX04_ErrorHandling_RetryPolicy;

public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly IDelayProvider _delayProvider;
    private readonly Type[] _retriableExceptions;

    public RetryPolicy(
        int maxRetries,
        TimeSpan initialDelay,
        IDelayProvider delayProvider,
        params Type[] retriableExceptions)
    {
        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _delayProvider = delayProvider;
        _retriableExceptions = retriableExceptions;
    }

    /// <summary>
    /// Executes the given async action with retry logic.
    /// - Retries up to <see cref="_maxRetries"/> times on retriable exceptions.
    /// - Uses exponential backoff: initialDelay, initialDelay*2, initialDelay*4, ...
    /// - If retriableExceptions is empty, retries on ALL exceptions.
    /// - Non-retriable exceptions are rethrown immediately.
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        Exception? lastException = null;
        for (int attempt = 0; attempt <= _maxRetries; attempt++)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                lastException = ex;
                if (!IsRetriable(ex))
                    throw;

                if (attempt == _maxRetries)
                    throw;

                var delay = TimeSpan.FromTicks(_initialDelay.Ticks * (1 << attempt));
                await _delayProvider.DelayAsync(delay);
            }
        }
        throw lastException!;

    }

    private bool IsRetriable(Exception ex)
    {
        if (_retriableExceptions.Length == 0)
            return true;

        var thrownType = ex.GetType();
        return _retriableExceptions.Any(t => t.IsAssignableFrom(thrownType));
    }

}
