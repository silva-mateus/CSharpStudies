namespace IX07_DI_NotificationService.Channels;

public class SmsChannel : INotificationChannel
{
    public NotificationChannel Channel => NotificationChannel.SMS;

    public Task SendAsync(Notification notification)
    {
        // TODO: Simulate sending an SMS (write to console).
        throw new NotImplementedException();
    }
}
