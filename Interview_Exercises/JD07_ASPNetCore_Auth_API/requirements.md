# JD07 - ASP.NET Core API with JWT Authentication

## Difficulty: Medium
## Estimated Time: 60-90 minutes
## Type: Create from scratch (with integration tests)

## Overview

Build a Task Management API with JWT-based authentication and role-based authorization using ASP.NET Core. This exercise tests your knowledge of the ASP.NET Core middleware pipeline, authentication/authorization, and integration testing of secured endpoints.

## Requirements

### Authentication

#### `POST /auth/register`
- Body: `{ "username": "...", "email": "...", "password": "...", "role": "User" | "Admin" }`
- Validates: username non-empty, email format, password min 8 chars
- Stores user in-memory (no database required)
- Passwords must be hashed (use `BCrypt` or simple SHA256 for exercise purposes)
- Returns: 201 with user info (no password) or 400 with errors

#### `POST /auth/login`
- Body: `{ "username": "...", "password": "..." }`
- Validates credentials against stored users
- Returns: 200 with `{ "token": "jwt-token-here" }` or 401
- Token contains claims: `sub` (username), `role` ("User" or "Admin"), `exp` (1 hour)

### Task Endpoints (all require `[Authorize]`)

| Method | Route | Authorization | Description |
|--------|-------|--------------|-------------|
| GET | `/tasks` | User: own tasks only. Admin: all tasks. | List tasks with optional `?status=` filter |
| GET | `/tasks/{id}` | Owner or Admin | Get single task |
| POST | `/tasks` | Any authenticated user | Create task (auto-assigned to current user) |
| PUT | `/tasks/{id}` | Owner or Admin | Update task |
| DELETE | `/tasks/{id}` | Owner or Admin | Delete task |

### Task Model
| Property | Type | Description |
|----------|------|-------------|
| Id | int | Auto-generated |
| Title | string | Required, 1-200 chars |
| Description | string? | Optional |
| Status | string | "Todo", "InProgress", "Done" |
| OwnerUsername | string | Set from JWT claim on creation |
| CreatedAt | DateTime | UTC |

### Middleware Pipeline

Configure in this order:
1. Global exception handling middleware (returns ProblemDetails for unhandled exceptions)
2. Authentication (`AddAuthentication().AddJwtBearer(...)`)
3. Authorization
4. Request logging middleware (log method, path, status code, duration)

### JWT Configuration

- Use a configurable signing key (from `appsettings.json` or hardcoded for exercise)
- Issuer: "JD07-TaskAPI"
- Audience: "JD07-TaskAPI"
- Expiration: 1 hour

## Integration Tests to Write

Using `WebApplicationFactory<Program>`:

1. Anonymous GET /tasks returns 401.
2. Register + Login returns a valid JWT token.
3. Authenticated GET /tasks returns 200.
4. User can only see their own tasks, not other users'.
5. Admin can see all tasks.
6. User cannot access another user's task by ID (returns 403 or 404).
7. POST /tasks creates a task owned by the authenticated user.
8. Login with wrong password returns 401.
9. Expired token returns 401 (bonus: mock time if feasible).

## Constraints

- Use ASP.NET Core Minimal APIs or Controllers (your choice).
- In-memory storage only (no database).
- Use `Microsoft.AspNetCore.Authentication.JwtBearer`.
- `Program.cs` must have `public partial class Program { }` for testing.

## Topics Covered

- ASP.NET Core authentication and authorization
- JWT token generation and validation
- Role-based access control
- Middleware pipeline
- Custom middleware (logging, error handling)
- Integration testing secured APIs
- WebApplicationFactory
