using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using JD09_Integration_Testing_Suite;

namespace JD09_Integration_Testing_Suite.Tests;

/// <summary>
/// TODO: Write 10+ integration tests using WebApplicationFactory.
///
/// TESTS:
///  1. GET /books returns 200 with seeded books.
///  2. GET /books?available=true returns only books with available copies.
///  3. POST /books creates book and returns 201 with Location header.
///  4. POST /books with empty title returns 400.
///  5. GET /members/{id} includes active loans.
///  6. POST /loans borrows a book successfully (returns 201).
///  7. POST /loans for unavailable book returns 400.
///  8. POST /loans/{id}/return returns the book (200).
///  9. GET /loans/overdue returns only overdue loans.
/// 10. POST /members with duplicate email returns 409.
/// </summary>
public class LibraryApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public LibraryApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // TODO: Write your integration tests here
}
