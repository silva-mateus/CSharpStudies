namespace Assignment_Quote_Finder.DataAccess;

public interface IQuotesApiDataReader : IDisposable
{
    Task<string> ReadAsync(int page, int quotesPerPage);
}