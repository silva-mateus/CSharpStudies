namespace UserManagement;

// ISSUE: not using ASP.NET Core properly - this is a pseudo-controller
// In a real app, this would inherit from ControllerBase
public class UserController
{
    private UserService _service = new UserService(); // ISSUE: should be injected

    public object GetUser(int id)
    {
        var user = _service.GetUser(id);
        if (user == null)
            return new { Error = "Not found" }; // ISSUE: should return proper HTTP 404
        return user; // ISSUE: returns the full User including Password field
    }

    public object GetAllUsers()
    {
        try
        {
            return _service.GetAllUsers();
        }
        catch (Exception ex)
        {
            // ISSUE: returning exception details to client - information leakage
            return new { Error = ex.Message, StackTrace = ex.StackTrace };
        }
    }

    public object CreateUser(string username, string email, string password, string role)
    {
        try
        {
            _service.CreateUser(username, email, password, role);
            return new { Success = true };
        }
        catch (Exception)
        {
            // ISSUE: swallowing exception, no logging, generic error
            return new { Success = false, Error = "Something went wrong" };
        }
    }

    public object Login(string username, string password)
    {
        // ISSUE: no rate limiting, no brute force protection
        var result = _service.Authenticate(username, password);
        if (result)
            return new { Token = "fake-jwt-token-" + username }; // ISSUE: fake token, not actual JWT
        return new { Error = "Invalid credentials" };
    }

    public object DeleteUser(int id)
    {
        // ISSUE: no authorization check - any user can delete any user
        _service.DeleteUser(id);
        return new { Success = true };
    }
}
