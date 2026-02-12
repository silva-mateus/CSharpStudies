namespace IX04_ErrorHandling_RetryPolicy;

/// <summary>
/// Thrown when an action is attempted while the circuit breaker is in the Open state.
/// </summary>
public class CircuitBreakerOpenException : Exception
{
    public CircuitBreakerOpenException()
        : base("Circuit breaker is open. Requests are not allowed.") { }

    public CircuitBreakerOpenException(string message)
        : base(message) { }

    public CircuitBreakerOpenException(string message, Exception innerException)
        : base(message, innerException) { }
}
