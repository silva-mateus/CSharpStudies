using JD09_Integration_Testing_Suite;

var builder = WebApplication.CreateBuilder(args);

// Register repositories as singletons (in-memory)
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddSingleton<IMemberRepository, InMemoryMemberRepository>();
builder.Services.AddSingleton<ILoanRepository, InMemoryLoanRepository>();
builder.Services.AddSingleton<IFineRepository, InMemoryFineRepository>();
builder.Services.AddTransient<LoanService>();

var app = builder.Build();

// Seed sample data
using (var scope = app.Services.CreateScope())
{
    var books = scope.ServiceProvider.GetRequiredService<IBookRepository>();
    books.Add(new Book { Title = "Clean Code", Author = "Robert C. Martin", ISBN = "9780132350884", TotalCopies = 3 });
    books.Add(new Book { Title = "Design Patterns", Author = "Gang of Four", ISBN = "9780201633610", TotalCopies = 2 });
    books.Add(new Book { Title = "C# in Depth", Author = "Jon Skeet", ISBN = "9781617294532", TotalCopies = 1 });

    var members = scope.ServiceProvider.GetRequiredService<IMemberRepository>();
    members.Add(new Member { Name = "Alice Johnson", Email = "alice@library.com" });
    members.Add(new Member { Name = "Bob Smith", Email = "bob@library.com" });
}

// Books endpoints
app.MapGet("/books", (IBookRepository repo, string? author, bool? available) =>
    Results.Ok(repo.GetAll(author, available)));

app.MapGet("/books/{id}", (int id, IBookRepository repo) =>
{
    var book = repo.GetById(id);
    return book is null ? Results.NotFound() : Results.Ok(book);
});

app.MapPost("/books", (CreateBookRequest request, IBookRepository repo) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest(new { Error = "Title is required." });
    if (request.TotalCopies < 1)
        return Results.BadRequest(new { Error = "Must have at least 1 copy." });

    var book = new Book
    {
        Title = request.Title,
        Author = request.Author,
        ISBN = request.ISBN,
        TotalCopies = request.TotalCopies
    };
    var created = repo.Add(book);
    return Results.Created($"/books/{created.Id}", created);
});

// Members endpoints
app.MapGet("/members", (IMemberRepository repo) => Results.Ok(repo.GetAll()));

app.MapGet("/members/{id}", (int id, IMemberRepository repo, ILoanRepository loans) =>
{
    var member = repo.GetById(id);
    if (member is null) return Results.NotFound();
    var activeLoans = loans.GetByMember(id).Where(l => !l.IsReturned).ToList();
    return Results.Ok(new { Member = member, ActiveLoans = activeLoans });
});

app.MapPost("/members", (CreateMemberRequest request, IMemberRepository repo) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
        return Results.BadRequest(new { Error = "Name is required." });
    if (string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest(new { Error = "Email is required." });
    if (repo.GetByEmail(request.Email) is not null)
        return Results.Conflict(new { Error = "Email already registered." });

    var member = new Member { Name = request.Name, Email = request.Email };
    var created = repo.Add(member);
    return Results.Created($"/members/{created.Id}", created);
});

// Loan endpoints
app.MapPost("/loans", (BorrowRequest request, LoanService loanService) =>
{
    var (success, error, loan) = loanService.Borrow(request.MemberId, request.BookId);
    if (!success)
        return Results.BadRequest(new { Error = error });
    return Results.Created($"/loans/{loan!.Id}", loan);
});

app.MapPost("/loans/{id}/return", (int id, LoanService loanService) =>
{
    var (success, error, fineAmount) = loanService.Return(id);
    if (!success)
        return Results.BadRequest(new { Error = error });
    return Results.Ok(new { Message = "Book returned.", FineAmount = fineAmount });
});

app.MapGet("/loans/overdue", (ILoanRepository repo) =>
    Results.Ok(repo.GetOverdue(DateTime.UtcNow)));

app.Run();

public partial class Program { }
