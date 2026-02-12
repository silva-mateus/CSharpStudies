using FluentAssertions;
using IX04_ErrorHandling_RetryPolicy;
using Xunit;
using Xunit.Abstractions;

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
/// </summary>
public class RetryPolicyTests
{
    private readonly RetryPolicy _retryPolicy;
    private readonly FakeDelayProvider _provider;
    private readonly TimeSpan _initialDelay;

    private record Messages
    {
        public const string ServiceUnavailable = "Service unavailable";
        public const string Success = "Success";
    }

    public RetryPolicyTests()
    {
        _initialDelay = TimeSpan.FromSeconds(1);
        _provider = new FakeDelayProvider();
        _retryPolicy = new RetryPolicy(
            maxRetries: 3,
            initialDelay: _initialDelay,
            delayProvider: _provider,
            typeof(HttpRequestException));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_OnZeroFailAttempts()
    {
        var result = await _retryPolicy.ExecuteAsync(SuccessOnXAttempts(0));

        result.Should().Be(Messages.Success);
        _provider.RecordedDelays.Should().BeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenFailTwiceThenSucceedOnThirdAttempt()
    {
        var result = await _retryPolicy.ExecuteAsync(SuccessOnXAttempts(2));

        result.Should().Be(Messages.Success);

        _provider.RecordedDelays[0].Should().Be(_initialDelay);
        _provider.RecordedDelays[1].Should().Be(TimeSpan.FromTicks(_initialDelay.Ticks * 2));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowLastException_WhenAllRetriesExhausted()
    {
        var act = () => _retryPolicy.ExecuteAsync(SuccessOnXAttempts(4));

        await act.Should().ThrowAsync<HttpRequestException>().WithMessage(Messages.ServiceUnavailable);


        _provider.RecordedDelays[0].Should().Be(_initialDelay);
        _provider.RecordedDelays[1].Should().Be(TimeSpan.FromTicks(_initialDelay.Ticks * 2));
        _provider.RecordedDelays[2].Should().Be(TimeSpan.FromTicks(_initialDelay.Ticks * 4));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowLastExceptionWithoutRetry_WhenNoRetriableExceptionsThrowed()
    {
        var retryPoliceWithArgumentException = new RetryPolicy(
            maxRetries: 3,
            initialDelay: _initialDelay,
            delayProvider: _provider,
            typeof(ArgumentException));
        
        var act = () => retryPoliceWithArgumentException.ExecuteAsync(SuccessOnXAttempts(4));

        await act.Should().ThrowAsync<HttpRequestException>().WithMessage(Messages.ServiceUnavailable);

        _provider.RecordedDelays.Should().BeEmpty();
    }

    private Func<Task<string>> SuccessOnXAttempts(int failCount)
    {
        var attempts = 0;
        return () =>
        {
            attempts++;
            if (attempts <= failCount)
                throw new HttpRequestException(Messages.ServiceUnavailable);

            return Task.FromResult(Messages.Success);
        };
    }





    //    var delayProvider = new SystemDelayProvider();

    //    // Demo: Retry Policy
    //    Console.WriteLine("=== Retry Policy Demo ===");
    //var retryPolicy = new RetryPolicy(
    //    maxRetries: 3,
    //    initialDelay: TimeSpan.FromSeconds(1),
    //    delayProvider: delayProvider,
    //    typeof(HttpRequestException));

    //    var attemptCount = 0;
    //try
    //{
    //    var result = await retryPolicy.ExecuteAsync<string>(async () =>
    //    {
    //        attemptCount++;
    //        Console.WriteLine($"  Attempt #{attemptCount}...");
    //        if (attemptCount < 3)
    //            throw new HttpRequestException("Service unavailable");
    //        return "Success!";
    //    });
    //    Console.WriteLine($"  Result: {result}");
    //}
    //catch (Exception ex)
    //{     
    //    Console.WriteLine($"  Failed after retries: {ex.Message}");
    //}
}
