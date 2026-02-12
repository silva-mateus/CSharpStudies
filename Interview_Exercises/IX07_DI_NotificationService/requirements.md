# IX07 - DI and Testability: Notification Service

## Difficulty: Medium
## Estimated Time: 60-90 minutes
## Type: Create from scratch (with unit tests using hand-rolled fakes)

## Overview

Design and implement a notification dispatch system where users have preferences for which channels (Email, SMS, Push) they want to receive notifications on. The system must be fully testable through dependency injection and hand-rolled fakes (no mocking libraries). This exercise tests your ability to design for testability from the ground up.

## Requirements

### Data Models (provided)

```csharp
public enum NotificationChannel { Email, SMS, Push }

public record Notification(string UserId, string Title, string Message);

public record UserPreference(string UserId, List<NotificationChannel> EnabledChannels);
```

### Interfaces to Define and Implement

#### `INotificationChannel`
- `NotificationChannel Channel { get; }` - Which channel this represents.
- `Task SendAsync(Notification notification)` - Sends the notification.

#### `IUserPreferenceRepository`
- `Task<UserPreference?> GetPreferencesAsync(string userId)` - Returns user preferences, or null if user not found.

#### `ILogger`
- `void LogInfo(string message)`
- `void LogError(string message, Exception? exception = null)`

### Implementations to Create

1. **`EmailChannel`** - Implements `INotificationChannel` for Email. Simulates sending by writing to console.
2. **`SmsChannel`** - Implements `INotificationChannel` for SMS. Simulates sending by writing to console.
3. **`PushChannel`** - Implements `INotificationChannel` for Push. Simulates sending by writing to console.
4. **`InMemoryUserPreferenceRepository`** - Stores preferences in a dictionary. Provide a way to seed data.
5. **`ConsoleLogger`** - Writes log messages to console.

### NotificationService

**Constructor**: Accepts `IEnumerable<INotificationChannel>`, `IUserPreferenceRepository`, and `ILogger`.

**Method**: `Task<NotificationResult> NotifyAsync(Notification notification)`

**Behavior**:
1. Retrieve user preferences. If user not found, log a warning and return a result indicating no channels notified.
2. Filter available channels to only those the user has enabled.
3. Send to ALL enabled channels concurrently.
4. If a channel throws an exception, catch it, log the error, and continue with remaining channels.
5. Return a `NotificationResult` with:
   - `int SuccessCount` - Number of channels that succeeded.
   - `int FailureCount` - Number of channels that failed.
   - `List<string> FailedChannels` - Names of channels that failed.

### NotificationResult (provided)

```csharp
public record NotificationResult(int SuccessCount, int FailureCount, List<string> FailedChannels);
```

## Unit Tests to Write

Using **hand-rolled fakes** (no Moq or NSubstitute), create tests covering:

1. **User with Email+SMS preferences** - Both channels are called.
2. **User with only Push preference** - Only Push channel is called, others are not.
3. **Unknown user** - Returns result with 0 successes, no exceptions thrown.
4. **Channel throws exception** - Other channels still receive the notification; failure is logged.
5. **All channels fail** - Result shows correct failure count and channel names.
6. **No matching channels available** - User prefers SMS but only Email channel is registered.

### Fakes to Create

- `FakeNotificationChannel` - Records all received notifications in a list. Can be configured to throw on send.
- `FakeUserPreferenceRepository` - Returns preconfigured preferences from a dictionary.
- `FakeLogger` - Records all log messages in a list for assertions.

## Constraints

- Do NOT use any mocking library (Moq, NSubstitute, etc.).
- All fakes must be hand-rolled classes implementing the interfaces.
- Use constructor injection exclusively.
- Use xUnit as the test framework.

## Topics Covered

- Dependency Injection (constructor injection)
- Interface design for testability
- Hand-rolled test fakes
- Async exception handling
- Concurrent async operations
- Logging abstraction
- Unit testing async code
