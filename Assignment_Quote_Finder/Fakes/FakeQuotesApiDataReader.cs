using Assignment_Quote_Finder.DataAccess;

namespace Assignment_Quote_Finder.Fakes;

public class FakeQuotesApiDataReader : IQuotesApiDataReader
{
    private readonly string _jsonToReturn;

    public FakeQuotesApiDataReader(string jsonToReturn)
    {
        _jsonToReturn = jsonToReturn;
    }

    public Task<string> ReadAsync(int page, int quotesPerPage)
    {
        return Task.FromResult(_jsonToReturn);
    }

    public void Dispose()
    {
    }

}

