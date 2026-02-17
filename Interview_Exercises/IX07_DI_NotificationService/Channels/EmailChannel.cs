namespace IX07_DI_NotificationService.Channels;

public class EmailChannel : INotificationChannel
{
    public NotificationChannel Channel => NotificationChannel.Email;

    public Task SendAsync(Notification notification)
    {
        Console.WriteLine($"[EMAIL] To: {notification.UserId} | {notification.Title}: {notification.Message}");
        return Task.CompletedTask;
    }
}
