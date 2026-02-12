namespace IX07_DI_NotificationService;

public interface INotificationChannel
{
    NotificationChannel Channel { get; }
    Task SendAsync(Notification notification);
}

public interface IUserPreferenceRepository
{
    Task<UserPreference?> GetPreferencesAsync(string userId);
}

public interface ILogger
{
    void LogInfo(string message);
    void LogError(string message, Exception? exception = null);
}
