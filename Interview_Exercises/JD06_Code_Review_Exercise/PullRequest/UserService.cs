namespace UserManagement;

public class UserService
{
    // ISSUE: tight coupling to concrete class, no interface
    private UserRepository _repo = new UserRepository();
    // ISSUE: static mutable cache, not thread-safe, no expiration
    private static Dictionary<int, User> _cache = new Dictionary<int, User>();

    public User? GetUser(int id)
    {
        // ISSUE: cache never invalidated, stale data
        if (_cache.ContainsKey(id))
            return _cache[id];

        var user = _repo.GetById(id);
        if (user != null)
            _cache[id] = user;
        return user;
    }

    public List<User> GetAllUsers()
    {
        // ISSUE: no caching, no pagination, loads everything into memory
        return _repo.GetAll();
    }

    public void CreateUser(string username, string email, string password, string role)
    {
        // ISSUE: no input validation (email format, password strength, role values)
        // ISSUE: password stored in plaintext - should be hashed
        var user = new User
        {
            Username = username,
            Email = email,
            Password = password,
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.Now  // ISSUE: should be DateTime.UtcNow
        };
        _repo.Create(user);
    }

    public bool Authenticate(string username, string password)
    {
        // ISSUE: N+1 - loads ALL users just to find one
        var users = _repo.GetAll();
        var user = users.FirstOrDefault(u => u.Username == username);

        if (user == null)
            return false;

        // ISSUE: plaintext password comparison
        if (user.Password == password)
        {
            user.LastLoginAt = DateTime.Now;
            // ISSUE: LastLoginAt update never saved to database
            return true;
        }

        return false;
    }

    public void DeleteUser(int id)
    {
        _repo.Delete(id);
        // ISSUE: cache not invalidated after delete
    }

    // ISSUE: no method to update user, change password, deactivate, etc.
}
