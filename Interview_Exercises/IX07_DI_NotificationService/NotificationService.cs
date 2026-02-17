namespace IX07_DI_NotificationService;

public class NotificationService
{
    private readonly IEnumerable<INotificationChannel> _channels;
    private readonly IUserPreferenceRepository _preferenceRepository;
    private readonly ILogger _logger;

    public NotificationService(
        IEnumerable<INotificationChannel> channels,
        IUserPreferenceRepository preferenceRepository,
        ILogger logger)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));
        _preferenceRepository = preferenceRepository ?? throw new ArgumentNullException(nameof(preferenceRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        if (notification == null)
            throw new ArgumentNullException(nameof(notification));

        var preferences = await _preferenceRepository.GetPreferencesAsync(notification.UserId);

        if (preferences == null)
        {
            _logger.LogInfo($"Preferences not found for user: [{notification.UserId}].");
            return new NotificationResult(0, 0, new List<string>());
        }

        var enabledChannels = _channels
            .Where(c => preferences.EnabledChannels.Contains(c.Channel))
            .ToList();

        if (!enabledChannels.Any())
        {
            _logger.LogInfo($"No matching channels for user {notification.UserId}");
            return new NotificationResult(0, 0, new List<string>());
        }

        var sendTasks = enabledChannels.Select(async channel =>
        {
            try
            {
                await channel.SendAsync(notification);
                return (Success: true, Channel: channel.Channel.ToString(), Error: (Exception?)null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send via {channel.Channel}", ex);
                return (Success: false, Channel: channel.Channel.ToString(), Error: ex);
            }
        });

        var results = await Task.WhenAll(sendTasks);

        var successCount = results.Count(r => r.Success);
        var failureCount = results.Count(r => !r.Success);
        var failureChannels = results
            .Where(r => !r.Success)
            .Select(r=> r.Channel)
            .ToList();

        return new NotificationResult(successCount, failureCount, failureChannels);
    }
}
