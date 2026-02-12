namespace IX07_DI_NotificationService.Channels;

public class EmailChannel : INotificationChannel
{
    public NotificationChannel Channel => NotificationChannel.Email;

    public Task SendAsync(Notification notification)
    {
        // TODO: Simulate sending an email (write to console).
        // Example: Console.WriteLine($"[EMAIL] To: {notification.UserId} | {notification.Title}: {notification.Message}");
        throw new NotImplementedException();
    }
}
