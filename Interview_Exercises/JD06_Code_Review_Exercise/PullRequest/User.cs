namespace UserManagement;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }    // ISSUE: no null initialization, no validation
    public string Email { get; set; }
    public string Password { get; set; }    // ISSUE: storing plaintext password
    public string Role { get; set; }        // ISSUE: magic string instead of enum
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
