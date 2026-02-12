using IX07_DI_NotificationService;
using IX07_DI_NotificationService.Channels;

// Composition root
var channels = new INotificationChannel[]
{
    new EmailChannel(),
    new SmsChannel(),
    new PushChannel()
};

var prefRepo = new InMemoryUserPreferenceRepository();
prefRepo.AddPreference(new UserPreference("user1", new List<NotificationChannel>
{
    NotificationChannel.Email,
    NotificationChannel.Push
}));
prefRepo.AddPreference(new UserPreference("user2", new List<NotificationChannel>
{
    NotificationChannel.SMS
}));

var logger = new ConsoleLogger();
var service = new NotificationService(channels, prefRepo, logger);

// Send notification to user1 (Email + Push)
Console.WriteLine("=== Notifying user1 ===");
var result1 = await service.NotifyAsync(new Notification("user1", "Welcome!", "Hello, user1!"));
Console.WriteLine($"Result: {result1.SuccessCount} sent, {result1.FailureCount} failed\n");

// Send notification to user2 (SMS only)
Console.WriteLine("=== Notifying user2 ===");
var result2 = await service.NotifyAsync(new Notification("user2", "Alert", "Important update."));
Console.WriteLine($"Result: {result2.SuccessCount} sent, {result2.FailureCount} failed\n");

// Send notification to unknown user
Console.WriteLine("=== Notifying unknown_user ===");
var result3 = await service.NotifyAsync(new Notification("unknown_user", "Test", "This should fail gracefully."));
Console.WriteLine($"Result: {result3.SuccessCount} sent, {result3.FailureCount} failed");
