using System.Text.Json;
using Assignment_Quote_Finder.Models;

namespace Assignment_Quote_Finder.DataAccess.Mock;

//this class may be used to get quotes in case Quotes Garden API is down
//it can be used where QuotesApiDataReader is used,
//as they implement the same interface
//it generates quotes using 500 most common English words
//so, of course, the quotes are nonesense
public class MockQuotesApiDataReader : IQuotesApiDataReader
{
    private readonly Random _random = new Random();

    public Task<string> ReadAsync(int page, int quotesPerPage)
    {
        var root = new QuoteResponse
        {
            Data = GenerateRandomData(quotesPerPage)
        };

        return Task.FromResult(JsonSerializer.Serialize(root));
    }

    private List<Quote> GenerateRandomData(int quotesPerPage)
    {
        return Enumerable.Range(0, quotesPerPage).Select(i =>
        new Quote
        {
            Id = i.ToString(),
            QuoteText = GenerateRandomQuote(),
            QuoteAuthor = "Unknown"
        }).ToList();
    }

    private string GenerateRandomQuote()
    {
        var length = _random.Next(5, 30);

        return string
            .Join(" ", Enumerable.Range(0, length)
            .Select(i => GetRandomWord()));
    }

    private string GetRandomWord()
    {
        var index = _random.Next(0, Words.All.Length);
        return Words.All[index];
    }

    public void Dispose()
    {
    }
}
