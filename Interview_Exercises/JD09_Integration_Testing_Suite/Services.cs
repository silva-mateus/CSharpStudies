namespace JD09_Integration_Testing_Suite;

/// <summary>
/// Interfaces for the service layer. Use these to create fakes for unit testing.
/// DO NOT MODIFY.
/// </summary>
public interface IBookRepository
{
    List<Book> GetAll(string? author = null, bool? available = null);
    Book? GetById(int id);
    Book Add(Book book);
    void UpdateAvailability(int bookId, int change);
}

public interface IMemberRepository
{
    List<Member> GetAll();
    Member? GetById(int id);
    Member? GetByEmail(string email);
    Member Add(Member member);
}

public interface ILoanRepository
{
    List<Loan> GetByMember(int memberId);
    List<Loan> GetOverdue(DateTime asOf);
    Loan? GetById(int id);
    Loan Add(Loan loan);
    void Update(Loan loan);
    int GetActiveLoanCount(int memberId);
}

public interface IFineRepository
{
    void Add(Fine fine);
    decimal GetUnpaidTotal(int memberId);
}

public static class FineCalculator
{
    public const decimal DailyRate = 0.50m;
    public const int LoanPeriodDays = 14;

    public static decimal Calculate(DateTime dueDate, DateTime returnDate)
    {
        var daysLate = (returnDate.Date - dueDate.Date).Days;
        return daysLate > 0 ? daysLate * DailyRate : 0m;
    }
}

public class LoanService
{
    private readonly IBookRepository _books;
    private readonly IMemberRepository _members;
    private readonly ILoanRepository _loans;
    private readonly IFineRepository _fines;

    public const int MaxActiveLoans = 5;
    public const decimal MaxUnpaidFinesForBorrowing = 10.00m;

    public LoanService(IBookRepository books, IMemberRepository members,
                       ILoanRepository loans, IFineRepository fines)
    {
        _books = books;
        _members = members;
        _loans = loans;
        _fines = fines;
    }

    public (bool Success, string? Error, Loan? Loan) Borrow(int memberId, int bookId)
    {
        var member = _members.GetById(memberId);
        if (member is null) return (false, "Member not found.", null);

        var book = _books.GetById(bookId);
        if (book is null) return (false, "Book not found.", null);

        if (book.AvailableCopies <= 0)
            return (false, "No copies available.", null);

        if (_loans.GetActiveLoanCount(memberId) >= MaxActiveLoans)
            return (false, $"Member already has {MaxActiveLoans} active loans.", null);

        if (_fines.GetUnpaidTotal(memberId) > MaxUnpaidFinesForBorrowing)
            return (false, "Member has unpaid fines exceeding $10.00.", null);

        var loan = new Loan
        {
            MemberId = memberId,
            BookId = bookId,
            LoanDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(FineCalculator.LoanPeriodDays),
            MemberName = member.Name,
            BookTitle = book.Title
        };

        _books.UpdateAvailability(bookId, -1);
        var created = _loans.Add(loan);
        return (true, null, created);
    }

    public (bool Success, string? Error, decimal FineAmount) Return(int loanId, DateTime? returnDate = null)
    {
        var loan = _loans.GetById(loanId);
        if (loan is null) return (false, "Loan not found.", 0);
        if (loan.IsReturned) return (false, "Loan already returned.", 0);

        var actualReturnDate = returnDate ?? DateTime.UtcNow;
        loan.ReturnDate = actualReturnDate;
        _loans.Update(loan);
        _books.UpdateAvailability(loan.BookId, +1);

        var fineAmount = FineCalculator.Calculate(loan.DueDate, actualReturnDate);
        if (fineAmount > 0)
        {
            _fines.Add(new Fine
            {
                LoanId = loanId,
                MemberId = loan.MemberId,
                Amount = fineAmount,
                IsPaid = false
            });
        }

        return (true, null, fineAmount);
    }
}
