namespace JD09_Integration_Testing_Suite;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}

public class Loan
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned => ReturnDate.HasValue;
    public string? MemberName { get; set; }
    public string? BookTitle { get; set; }
}

public class Fine
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public int MemberId { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
}

public record BorrowRequest(int MemberId, int BookId);
public record CreateBookRequest(string Title, string Author, string ISBN, int TotalCopies);
public record CreateMemberRequest(string Name, string Email);
