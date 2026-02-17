namespace IX07_DI_NotificationService;

public class InMemoryUserPreferenceRepository : IUserPreferenceRepository
{
    private readonly Dictionary<string, UserPreference> _preferences = new();

    public void AddPreference(UserPreference preference)
    {
        _preferences[preference.UserId] = preference;
    }

    public Task<UserPreference?> GetPreferencesAsync(string userId)
    {
        _preferences.TryGetValue(userId, out var preference);
        return Task.FromResult(preference);
    }
}
