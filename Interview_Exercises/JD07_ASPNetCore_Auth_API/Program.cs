using JD07_ASPNetCore_Auth_API;

var builder = WebApplication.CreateBuilder(args);

// TODO: Configure services:
// 1. Add Authentication with JwtBearer
//    - SigningKey from configuration or hardcoded (e.g., "this-is-a-secret-key-for-jd07-exercise-minimum-32-chars!")
//    - Issuer: "JD07-TaskAPI", Audience: "JD07-TaskAPI"
//    - ValidateIssuer, ValidateAudience, ValidateLifetime, ValidateIssuerSigningKey = true
// 2. Add Authorization with policies:
//    - "AdminOnly" policy requiring role "Admin"
// 3. Register in-memory stores as singletons:
//    - UserStore (for registered users)
//    - TaskStore (for task items)
// 4. Register a JwtTokenService for generating tokens

var app = builder.Build();

// TODO: Configure middleware pipeline in order:
// 1. Global exception handling middleware
// 2. Authentication
// 3. Authorization
// 4. Request logging middleware (optional)

// TODO: Map auth endpoints:
// POST /auth/register -> validate, hash password, store user, return 201
// POST /auth/login -> validate credentials, generate JWT, return 200 with token

// TODO: Map task endpoints (all require [Authorize]):
// GET /tasks -> User sees own tasks, Admin sees all. Optional ?status= filter.
// GET /tasks/{id} -> Owner or Admin only
// POST /tasks -> Create task, set OwnerUsername from JWT claims
// PUT /tasks/{id} -> Owner or Admin only
// DELETE /tasks/{id} -> Owner or Admin only

app.Run();

// Required for WebApplicationFactory in integration tests
public partial class Program { }
