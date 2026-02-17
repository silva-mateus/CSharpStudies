namespace IX07_DI_NotificationService.Channels;

public class SmsChannel : INotificationChannel
{
    public NotificationChannel Channel => NotificationChannel.SMS;

    public Task SendAsync(Notification notification)
    {
        Console.WriteLine($"[SMS] To: {notification.UserId} | {notification.Title}: {notification.Message}");
        return Task.CompletedTask;
    }
}
