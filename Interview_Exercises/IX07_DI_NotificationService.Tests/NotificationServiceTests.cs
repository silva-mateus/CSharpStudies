using IX07_DI_NotificationService;

namespace IX07_DI_NotificationService.Tests;

/// <summary>
/// TODO: Create the following hand-rolled fakes and write the tests below.
///
/// FAKES TO CREATE (in separate files or in this file):
///
/// 1. FakeNotificationChannel : INotificationChannel
///    - Records all received notifications in a public List.
///    - Has a ShouldThrow property; when true, throws Exception on SendAsync.
///    - Constructor takes the NotificationChannel enum value.
///
/// 2. FakeUserPreferenceRepository : IUserPreferenceRepository
///    - Stores preferences in a Dictionary.
///    - Has an AddPreference method for test setup.
///
/// 3. FakeLogger : ILogger
///    - Records all log messages in public Lists (InfoMessages, ErrorMessages).
///
/// TESTS TO WRITE:
///
/// 1. User with Email+SMS preferences - both channels receive the notification.
/// 2. User with only Push preference - only Push channel is called.
/// 3. Unknown user - returns NotificationResult with 0 successes, 0 failures.
/// 4. One channel throws - other channels still receive; failure is counted and logged.
/// 5. All channels fail - result shows correct failure count and channel names.
/// 6. User prefers SMS but only Email channel is registered - 0 successes (no matching channel).
/// </summary>
public class NotificationServiceTests
{
    // TODO: Create fakes and write tests here
}
