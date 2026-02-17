using IX07_DI_NotificationService;
using Xunit;

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
/// 

public class FakeLogger : ILogger
{
    public List<string> InfoMessages { get; } = new();
    public List<(string message, Exception? ex)> ErrorMessages { get; } = new();

    public void LogInfo(string message) => InfoMessages.Add(message);
    public void LogError(string message, Exception? exception = null) => ErrorMessages.Add((message, exception));

}

public class FakeUserPreferenceRepository : IUserPreferenceRepository
{
    private readonly Dictionary<string, UserPreference> _preferences = new();
    public void AddPreference(UserPreference preference)
    {
        _preferences[preference.UserId] = preference;
    }

    public Task<UserPreference?> GetPreferencesAsync(string userId)
    {
        _preferences.TryGetValue(userId, out var preference);
        return Task.FromResult(preference);
    }
}

public class FakeNotificationChannel : INotificationChannel
{
    public NotificationChannel Channel { get; }
    public List<Notification> SentNotifications { get; } = new();
    public FakeNotificationChannel(NotificationChannel channel)
    {
        Channel = channel;
    }

    public bool ShouldThrow { get; set; }

    public Task SendAsync(Notification notification)
    {
        if (ShouldThrow)
            throw new Exception($"{Channel} channel failed!");

        SentNotifications.Add(notification);
        return Task.CompletedTask;
        
    }
}
public class NotificationServiceTests
{
    [Fact]
    public async Task NotifyAsync_UserWithEmailAndSms_BothChannelsReceiveNotification()
    {
        var emailChannel = new FakeNotificationChannel(NotificationChannel.Email);
        var smsChannel = new FakeNotificationChannel(NotificationChannel.SMS);
        var pushChannel = new FakeNotificationChannel(NotificationChannel.Push);

        var repo = new FakeUserPreferenceRepository();

        repo.AddPreference(new UserPreference("user1", new List<NotificationChannel> { NotificationChannel.Email, NotificationChannel.SMS }));

        var logger = new FakeLogger();

        var service = new NotificationService(
            new[] { emailChannel, smsChannel, pushChannel },
            repo,
            logger);

        var notification = new Notification("user1", "Test", "Message");

        var result = await service.NotifyAsync(notification);

        Assert.Equal(2, result.SuccessCount);
        Assert.Equal(0, result.FailureCount);
        Assert.Single(emailChannel.SentNotifications);
        Assert.Single(smsChannel.SentNotifications);
        Assert.Empty(pushChannel.SentNotifications);
    }

    [Fact]
    public async Task NotifyAsync_UserWithPush_OnlyPushChannelReceiveNotification()
    {
        var emailChannel = new FakeNotificationChannel(NotificationChannel.Email);
        var smsChannel = new FakeNotificationChannel(NotificationChannel.SMS);
        var pushChannel = new FakeNotificationChannel(NotificationChannel.Push);

        var repo = new FakeUserPreferenceRepository();

        repo.AddPreference(new UserPreference("user1", new List<NotificationChannel> { NotificationChannel.Push }));

        var logger = new FakeLogger();

        var service = new NotificationService(
            new[] { emailChannel, smsChannel, pushChannel },
            repo,
            logger);

        var notification = new Notification("user1", "Test", "Message");

        var result = await service.NotifyAsync(notification);

        Assert.Equal(1, result.SuccessCount);
        Assert.Equal(0, result.FailureCount);
        Assert.Single(pushChannel.SentNotifications);
        Assert.Empty(emailChannel.SentNotifications);
        Assert.Empty(smsChannel.SentNotifications);
    }

    [Fact]
    public async Task NotifyAsync_UnknownUser_ReturnNotificationResultWith0SuccessesAnd0Failures()
    {
        var emailChannel = new FakeNotificationChannel(NotificationChannel.Email);
        var smsChannel = new FakeNotificationChannel(NotificationChannel.SMS);
        var pushChannel = new FakeNotificationChannel(NotificationChannel.Push);

        var repo = new FakeUserPreferenceRepository();

        repo.AddPreference(new UserPreference("user1", new List<NotificationChannel> { NotificationChannel.Push }));

        var logger = new FakeLogger();

        var service = new NotificationService(
            new[] { emailChannel, smsChannel, pushChannel },
            repo,
            logger);

        var notification = new Notification("unknown", "Test", "Message");

        var result = await service.NotifyAsync(notification);

        Assert.Equal(0, result.SuccessCount);
        Assert.Equal(0, result.FailureCount);
        Assert.Empty(pushChannel.SentNotifications);
        Assert.Empty(emailChannel.SentNotifications);
        Assert.Empty(smsChannel.SentNotifications);
        Assert.Contains(logger.InfoMessages, e => e.Contains("Preferences not found for user: [unknown]"));
    }

    [Fact]
    public async Task NotifyAsync_EmailShouldThrow_SMSAndPushNotificationsReceiveNotification()
    {
        var emailChannel = new FakeNotificationChannel(NotificationChannel.Email);
        emailChannel.ShouldThrow = true;
        var smsChannel = new FakeNotificationChannel(NotificationChannel.SMS);
        var pushChannel = new FakeNotificationChannel(NotificationChannel.Push);

        var repo = new FakeUserPreferenceRepository();

        repo.AddPreference(new UserPreference("user1", new List<NotificationChannel> { NotificationChannel.Email, NotificationChannel.SMS, NotificationChannel.Push }));

        var logger = new FakeLogger();

        var service = new NotificationService(
            new[] { emailChannel, smsChannel, pushChannel },
            repo,
            logger);

        var notification = new Notification("user1", "Test", "Message");

        var result = await service.NotifyAsync(notification);

        Assert.Equal(2, result.SuccessCount);
        Assert.Equal(1, result.FailureCount);
        Assert.Single(pushChannel.SentNotifications);
        Assert.Empty(emailChannel.SentNotifications);
        Assert.Single(smsChannel.SentNotifications);
        Assert.Contains(logger.ErrorMessages,
            e => e.message.Contains(emailChannel.Channel.ToString()));
    }

    [Fact]
    public async Task NotifyAsync_AllShouldThrow_ShowsCorrectFailureCountAndChannelNames()
    {
        var emailChannel = new FakeNotificationChannel(NotificationChannel.Email);
        emailChannel.ShouldThrow = true;
        var smsChannel = new FakeNotificationChannel(NotificationChannel.SMS);
        smsChannel.ShouldThrow = true;
        var pushChannel = new FakeNotificationChannel(NotificationChannel.Push);
        pushChannel.ShouldThrow = true;

        var repo = new FakeUserPreferenceRepository();

        repo.AddPreference(new UserPreference("user1", new List<NotificationChannel> { NotificationChannel.Email, NotificationChannel.SMS, NotificationChannel.Push }));

        var logger = new FakeLogger();

        var service = new NotificationService(
            new[] { emailChannel, smsChannel, pushChannel },
            repo,
            logger);

        var notification = new Notification("user1", "Test", "Message");

        var result = await service.NotifyAsync(notification);

        Assert.Equal(0, result.SuccessCount);
        Assert.Equal(3, result.FailureCount);
        Assert.Empty(pushChannel.SentNotifications);
        Assert.Empty(emailChannel.SentNotifications);
        Assert.Empty(smsChannel.SentNotifications);
        Assert.Contains(logger.ErrorMessages,
            e => e.message.Contains(emailChannel.Channel.ToString()));
        Assert.Contains(logger.ErrorMessages,
            e => e.message.Contains(smsChannel.Channel.ToString()));
        Assert.Contains(logger.ErrorMessages,
            e => e.message.Contains(pushChannel.Channel.ToString()));
    }

    [Fact]
    public async Task NotifyAsync_UserPrefersSMSButOnlyEmailChannelIsRegistered_ZeroSuccessesNoMachingChannel()
    {
        var emailChannel = new FakeNotificationChannel(NotificationChannel.Email);

        var repo = new FakeUserPreferenceRepository();

        repo.AddPreference(new UserPreference("user1", new List<NotificationChannel> { NotificationChannel.SMS }));

        var logger = new FakeLogger();

        var service = new NotificationService(
            new[] { emailChannel },
            repo,
            logger);

        var notification = new Notification("user1", "Test", "Message");

        var result = await service.NotifyAsync(notification);

        Assert.Equal(0, result.SuccessCount);
        Assert.Empty(emailChannel.SentNotifications);
        Assert.Contains(logger.InfoMessages,
            e => e.Contains("No matching channels"));
    }
}
