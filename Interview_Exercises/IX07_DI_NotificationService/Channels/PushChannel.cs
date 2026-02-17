namespace IX07_DI_NotificationService.Channels;

public class PushChannel : INotificationChannel
{
    public NotificationChannel Channel => NotificationChannel.Push;

    public Task SendAsync(Notification notification)
    {
        Console.WriteLine($"[Push] notification To: {notification.UserId} | {notification.Title}: {notification.Message}");
        return Task.CompletedTask;
    }
}
