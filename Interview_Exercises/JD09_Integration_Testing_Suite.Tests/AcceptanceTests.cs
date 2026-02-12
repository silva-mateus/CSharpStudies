using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using JD09_Integration_Testing_Suite;

namespace JD09_Integration_Testing_Suite.Tests;

/// <summary>
/// TODO: Write 5+ acceptance tests as end-to-end Given/When/Then scenarios.
///
/// These test complete user journeys, not individual endpoints.
/// Use descriptive method names following the pattern:
///   Given_[precondition]_When_[action]_Then_[expected]
///
/// SCENARIOS:
///
/// 1. Given_MemberWithNoLoans_When_BorrowsBook_Then_LoanCreatedAndAvailabilityDecreases
///    - Create a member, note a book's available count
///    - POST /loans to borrow
///    - Verify loan is created
///    - GET /books/{id} to verify available copies decreased by 1
///
/// 2. Given_MemberBorrowedBook_When_ReturnsOnTime_Then_NoFineAndBookAvailable
///    - Borrow a book
///    - Return it immediately (within due date)
///    - Verify FineAmount = 0
///    - Verify book availability increased
///
/// 3. Given_MemberBorrowedBook_When_ReturnsLate_Then_FineIsCreated
///    - This requires the service to accept a custom return date,
///      or you test via the unit test layer.
///    - If testing via API, just verify the return endpoint works.
///
/// 4. Given_MemberWithFiveLoans_When_TriesToBorrow_Then_Rejected
///    - Create a member, borrow 5 different books
///    - Try to borrow a 6th
///    - Verify 400 response with appropriate error message
///
/// 5. Given_BookWithOneCopy_When_TwoPeopleTryToBorrow_Then_SecondIsRejected
///    - Create a book with 1 copy
///    - Member A borrows it (success)
///    - Member B tries to borrow it (failure)
/// </summary>
public class LibraryAcceptanceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public LibraryAcceptanceTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // TODO: Write your acceptance tests here
}
