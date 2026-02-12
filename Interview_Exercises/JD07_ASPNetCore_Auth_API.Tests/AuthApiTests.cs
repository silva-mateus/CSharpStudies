using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;

namespace JD07_ASPNetCore_Auth_API.Tests;

/// <summary>
/// TODO: Write integration tests for the Task Management API with JWT auth.
///
/// Helper pattern: Create a method to register and login a user, returning the JWT token.
///   async Task&lt;string&gt; GetTokenAsync(HttpClient client, string username, string role)
///
/// TESTS TO WRITE:
///  1. Anonymous GET /tasks returns 401 Unauthorized.
///  2. POST /auth/register + POST /auth/login returns a valid JWT token.
///  3. Authenticated GET /tasks returns 200.
///  4. User can only see their own tasks (User A creates task, User B cannot see it).
///  5. Admin can see all tasks.
///  6. User cannot GET /tasks/{id} for another user's task (403 or 404).
///  7. POST /tasks creates a task owned by the authenticated user (OwnerUsername from JWT).
///  8. POST /auth/login with wrong password returns 401.
///  9. (Bonus) Registration with invalid data returns 400 with validation errors.
/// </summary>
public class AuthApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // TODO: Write your integration tests here
}
