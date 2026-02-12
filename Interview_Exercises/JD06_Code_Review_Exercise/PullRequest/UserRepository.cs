using System.Data.SqlClient;

namespace UserManagement;

public class UserRepository
{
    private readonly string _connectionString = "Server=prod-db.company.com;Database=UsersDB;User=sa;Password=Admin123!"; // ISSUE: hardcoded connection string with credentials

    public User? GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        // ISSUE: SQL injection - string concatenation instead of parameters
        var command = new SqlCommand($"SELECT * FROM Users WHERE Id = {id}", connection);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new User
            {
                Id = (int)reader["Id"],
                Username = (string)reader["Username"],
                Email = (string)reader["Email"],
                Password = (string)reader["Password"],  // ISSUE: reading password from DB and exposing it
                Role = (string)reader["Role"],
                IsActive = (bool)reader["IsActive"]
            };
        }
        return null;
    }

    public List<User> GetAll()
    {
        var users = new List<User>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand("SELECT * FROM Users", connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User
            {
                Id = (int)reader["Id"],
                Username = (string)reader["Username"],
                Email = (string)reader["Email"],
                Password = (string)reader["Password"],
                Role = (string)reader["Role"],
                IsActive = (bool)reader["IsActive"]
            });
        }
        return users;
    }

    // ISSUE: No async methods, blocking I/O
    public void Create(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        // ISSUE: SQL injection again
        var sql = $"INSERT INTO Users (Username, Email, Password, Role, IsActive, CreatedAt) VALUES ('{user.Username}', '{user.Email}', '{user.Password}', '{user.Role}', {(user.IsActive ? 1 : 0)}, GETUTCDATE())";
        var command = new SqlCommand(sql, connection);
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand($"DELETE FROM Users WHERE Id = {id}", connection);
        command.ExecuteNonQuery();
        // ISSUE: no check if the user existed, no return value
    }
}
