using FluentAssertions;
using IX04_ErrorHandling_RetryPolicy;
using Xunit;

namespace IX04_ErrorHandling_RetryPolicy.Tests;

/// <summary>
/// TODO: Write unit tests for RetryPolicy and CircuitBreaker.
/// Use FakeDelayProvider to control time and verify delays.
///
/// CircuitBreaker tests:
///   6. Stays Closed when actions succeed.
///   7. Opens after consecutive failures reach the threshold.
///   8. Throws CircuitBreakerOpenException when Open.
///   9. Transitions to HalfOpen after openDuration elapses (use FakeDelayProvider.Advance).
///  10. Successful call in HalfOpen transitions back to Closed.
///  11. Failed call in HalfOpen transitions back to Open.
/// </summary>
public class CircuitBreakerTests
{
    private readonly CircuitBreaker _circuitBreaker;
    private readonly FakeDelayProvider _provider;
    private readonly int _openDurationInSeconds;

    private record Messages
    {
        public const string ServiceError = "Service error";
        public const string Success = "Success";
    }

    public CircuitBreakerTests()
    {
        _provider = new FakeDelayProvider();
        _openDurationInSeconds = 5;
        _circuitBreaker = new CircuitBreaker(
            failureThreshold: 3,
            openDuration: TimeSpan.FromSeconds(_openDurationInSeconds),
            delayProvider: _provider);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStayClosed_WhenActionsSucceed()
    {
        await _circuitBreaker.ExecuteAsync(AlwaysSucceeds());
        await _circuitBreaker.ExecuteAsync(AlwaysSucceeds());

        _circuitBreaker.State.Should().Be(CircuitState.Closed);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldOpen_AfterReachingFailureThreshold()
    {
        for (int i = 0; i < 3; i++)
        {
            try { await _circuitBreaker.ExecuteAsync(AlwaysFails()); }
            catch { }
        }

        _circuitBreaker.State.Should().Be(CircuitState.Open);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowCircuitBreakerOpenException_WhenOpen()
    {
        for (int i = 0; i < 3; i++)
        {
            try { await _circuitBreaker.ExecuteAsync(AlwaysFails()); }
            catch { }
        }

        var act = () => _circuitBreaker.ExecuteAsync(AlwaysSucceeds());

        await act.Should().ThrowAsync<CircuitBreakerOpenException>();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldTransitionFromHalfOpenToClosed_AfterOpenDurationElapsesAndSucceeds()
    {
        for (int i = 0; i < 3; i++)
        {
            try { await _circuitBreaker.ExecuteAsync(AlwaysFails()); }
            catch { }
        }

        _circuitBreaker.State.Should().Be(CircuitState.Open);

        _provider.Advance(TimeSpan.FromSeconds(_openDurationInSeconds + 1));

        // Half Open to Closed
        await _circuitBreaker.ExecuteAsync(AlwaysSucceeds());

        _circuitBreaker.State.Should().Be(CircuitState.Closed);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldTransitionFromHalfOpenToOpen_AfterOpenDurationElapsesAndFails()
    {
        for (int i = 0; i < 3; i++)
        {
            try { await _circuitBreaker.ExecuteAsync(AlwaysFails()); }
            catch { }
        }

        _circuitBreaker.State.Should().Be(CircuitState.Open);

        _provider.Advance(TimeSpan.FromSeconds(_openDurationInSeconds + 1));

        // Half Open to Open
        try { await _circuitBreaker.ExecuteAsync(AlwaysFails()); }
        catch { }

        _circuitBreaker.State.Should().Be(CircuitState.Open);
    }

    private Func<Task<string>> AlwaysSucceeds()
        => () => Task.FromResult(Messages.Success);

    private Func<Task<string>> AlwaysFails()
        => () => throw new Exception(Messages.ServiceError);


}
