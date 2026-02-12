namespace JD09_Integration_Testing_Suite;

/// <summary>
/// In-memory implementations of all repositories. DO NOT MODIFY.
/// </summary>
public class InMemoryBookRepository : IBookRepository
{
    private readonly List<Book> _books = new();
    private int _nextId = 1;

    public List<Book> GetAll(string? author = null, bool? available = null)
    {
        var query = _books.AsEnumerable();
        if (!string.IsNullOrEmpty(author))
            query = query.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        if (available == true)
            query = query.Where(b => b.AvailableCopies > 0);
        return query.ToList();
    }

    public Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

    public Book Add(Book book)
    {
        book.Id = _nextId++;
        book.AvailableCopies = book.TotalCopies;
        _books.Add(book);
        return book;
    }

    public void UpdateAvailability(int bookId, int change)
    {
        var book = GetById(bookId);
        if (book != null) book.AvailableCopies += change;
    }
}

public class InMemoryMemberRepository : IMemberRepository
{
    private readonly List<Member> _members = new();
    private int _nextId = 1;

    public List<Member> GetAll() => _members.ToList();
    public Member? GetById(int id) => _members.FirstOrDefault(m => m.Id == id);
    public Member? GetByEmail(string email) => _members.FirstOrDefault(m => m.Email == email);

    public Member Add(Member member)
    {
        member.Id = _nextId++;
        _members.Add(member);
        return member;
    }
}

public class InMemoryLoanRepository : ILoanRepository
{
    private readonly List<Loan> _loans = new();
    private int _nextId = 1;

    public List<Loan> GetByMember(int memberId) => _loans.Where(l => l.MemberId == memberId).ToList();

    public List<Loan> GetOverdue(DateTime asOf) =>
        _loans.Where(l => !l.IsReturned && l.DueDate < asOf).ToList();

    public Loan? GetById(int id) => _loans.FirstOrDefault(l => l.Id == id);

    public Loan Add(Loan loan)
    {
        loan.Id = _nextId++;
        _loans.Add(loan);
        return loan;
    }

    public void Update(Loan loan)
    {
        var existing = GetById(loan.Id);
        if (existing != null)
        {
            existing.ReturnDate = loan.ReturnDate;
        }
    }

    public int GetActiveLoanCount(int memberId) =>
        _loans.Count(l => l.MemberId == memberId && !l.IsReturned);
}

public class InMemoryFineRepository : IFineRepository
{
    private readonly List<Fine> _fines = new();
    private int _nextId = 1;

    public void Add(Fine fine)
    {
        fine.Id = _nextId++;
        _fines.Add(fine);
    }

    public decimal GetUnpaidTotal(int memberId) =>
        _fines.Where(f => f.MemberId == memberId && !f.IsPaid).Sum(f => f.Amount);
}
