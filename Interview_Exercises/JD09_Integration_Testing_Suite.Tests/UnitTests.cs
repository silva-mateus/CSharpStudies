using JD09_Integration_Testing_Suite;
using Xunit;
using FluentAssertions;

namespace JD09_Integration_Testing_Suite.Tests;

/// <summary>
/// TODO: Write 15+ unit tests for LoanService and FineCalculator.
/// Use hand-rolled fakes implementing the repository interfaces.
///
/// FAKES TO CREATE:
///   - FakeBookRepository : IBookRepository
///   - FakeMemberRepository : IMemberRepository
///   - FakeLoanRepository : ILoanRepository
///   - FakeFineRepository : IFineRepository
///
/// You can reuse InMemoryXxxRepository from the main project as your fakes,
/// or write dedicated fake implementations with more control.
///
/// TESTS (minimum 15):
///  1. Borrowing reduces available copies by 1.
///  2. Borrowing when no copies available returns error.
///  3. Member with 5 active loans cannot borrow.
///  4. Member with > $10 unpaid fines cannot borrow.
///  5. Returning a book increases available copies.
///  6. Returning on time generates no fine (FineAmount = 0).
///  7. Returning 1 day late generates $0.50 fine.
///  8. Returning 10 days late generates $5.00 fine.
///  9. Loan due date is 14 days from loan date.
/// 10. Returning already-returned loan returns error.
/// 11. Invalid book ID returns error.
/// 12. Invalid member ID returns error.
/// 13. FineCalculator with zero days late returns 0.
/// 14. FineCalculator with negative days (returned early) returns 0.
/// 15. Available copies calculation is correct after borrow + return.
/// </summary>
public class LoanServiceUnitTests
{
    // TODO: Write your unit tests here
}

public class FineCalculatorTests
{
    // TODO: Write FineCalculator tests here
}
