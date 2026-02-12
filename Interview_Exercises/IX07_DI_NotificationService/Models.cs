namespace IX07_DI_NotificationService;

public enum NotificationChannel
{
    Email,
    SMS,
    Push
}

public record Notification(string UserId, string Title, string Message);

public record UserPreference(string UserId, List<NotificationChannel> EnabledChannels);

public record NotificationResult(int SuccessCount, int FailureCount, List<string> FailedChannels);
