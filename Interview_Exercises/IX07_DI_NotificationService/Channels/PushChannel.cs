namespace IX07_DI_NotificationService.Channels;

public class PushChannel : INotificationChannel
{
    public NotificationChannel Channel => NotificationChannel.Push;

    public Task SendAsync(Notification notification)
    {
        // TODO: Simulate sending a push notification (write to console).
        throw new NotImplementedException();
    }
}
