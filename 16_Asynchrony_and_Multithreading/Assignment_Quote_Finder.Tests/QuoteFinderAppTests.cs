using Assignment_Quote_Finder.Fakes;
using Assignment_Quote_Finder.Models;
using System.Text.Json;

namespace Assignment_Quote_Finder.Tests;

[TestFixture]
public class QuoteFinderAppTests
{
    private static string CreateQuotesJson(params string[] quoteTexts)
    {
        var response = new QuoteResponse
        {
            Data = quoteTexts.Select((text, i) => new Quote
            {
                Id = i.ToString(),
                QuoteText = text,
                QuoteAuthor = "Author"
            }).ToList()
        };
        return JsonSerializer.Serialize(response);
    }

    [Test]
    public async Task RunAsync_WordFoundOnPage_PrintsShortestQuote()
    {
        var json = CreateQuotesJson(
            "I have a dream",
            "I have popcorn and ice cream.",
            "To be or not to be"
        );

        var fakeReader = new FakeQuotesApiDataReader(json);

        var fakeUc = new FakeUserCommunication("have", "1", "10", "n");

        var app = new QuoteFinderApp(fakeUc, fakeReader, new QuoteSearchService());

        await app.RunAsync();

        Assert.That(
            fakeUc.PrintedMessages.Any(m => m.Contains("I have a dream")),
            Is.True
        );
    }

    [Test]
    public async Task RunAsync_NoQuotesMatchWord_PrintsNotFoundMessage()
    {
        var json = CreateQuotesJson(
            "To be or not to be",
            "The quick brown fox"
            );

        var fakeReader = new FakeQuotesApiDataReader(json);

        var fakeUc = new FakeUserCommunication("have", "1", "10", "n");

        var app = new QuoteFinderApp(fakeUc, fakeReader, new QuoteSearchService());

        await app.RunAsync();

        Assert.That(
            fakeUc.PrintedMessages.Any(m => m.Contains("no quotes found")),
            Is.True
        );
    }

    [Test]
    public async Task RunAsync_WordMatchIsWholeWord_DoesNotMatchPartialWords()
    {
        var json = CreateQuotesJson(
            "This is about a category of things",
            "This is my cat"
        );

        var fakeReader = new FakeQuotesApiDataReader(json);

        var fakeUc = new FakeUserCommunication("cat", "1", "10", "n");

        var app = new QuoteFinderApp(fakeUc, fakeReader, new QuoteSearchService());

        await app.RunAsync();

        Assert.That(
            fakeUc.PrintedMessages.Any(m => m.Contains("This is my cat")),
            Is.True
        );
        Assert.That(
            fakeUc.PrintedMessages.Any(m => m.Contains("category")),
            Is.False
        );
    }
}
