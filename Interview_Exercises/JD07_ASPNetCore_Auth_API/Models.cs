namespace JD07_ASPNetCore_Auth_API;

public record RegisterRequest(string Username, string Email, string Password, string Role);
public record LoginRequest(string Username, string Password);
public record LoginResponse(string Token);
public record UserResponse(string Username, string Email, string Role);

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Todo"; // "Todo", "InProgress", "Done"
    public string OwnerUsername { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public record CreateTaskRequest(string Title, string? Description, string Status = "Todo");
public record UpdateTaskRequest(string Title, string? Description, string Status);
