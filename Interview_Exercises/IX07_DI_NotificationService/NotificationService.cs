namespace IX07_DI_NotificationService;

public class NotificationService
{
    // TODO: Store injected dependencies as fields

    public NotificationService(
        IEnumerable<INotificationChannel> channels,
        IUserPreferenceRepository preferenceRepository,
        ILogger logger)
    {
        // TODO: Assign dependencies
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sends a notification to the user through their preferred channels.
    ///
    /// Steps:
    /// 1. Retrieve user preferences. If not found, log warning and return empty result.
    /// 2. Filter available channels to those the user has enabled.
    /// 3. Send to all enabled channels concurrently.
    /// 4. If a channel throws, catch, log error, continue with others.
    /// 5. Return NotificationResult with success/failure counts.
    /// </summary>
    public async Task<NotificationResult> NotifyAsync(Notification notification)
    {
        // TODO: your code goes here
        throw new NotImplementedException();
    }
}
