using Assignment_Quote_Finder.Fakes;
using Assignment_Quote_Finder.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Assignment_Quote_Finder.Tests;

[TestFixture]
public class QuoteSearchServiceTests
{
    private QuoteSearchService _service;
    private Regex _regex;

    [SetUp]
    public void SetUp()
    {
        _service = new QuoteSearchService();
    }

    [Test]
    public async Task FindShortestMatch_MultipleMatches_ReturnShortest()
    {
        _regex = new Regex(@"\bhave\b", RegexOptions.IgnoreCase);

        var quotes = new List<Quote>
        {
            new Quote { QuoteText = "I have a dream" },
            new Quote { QuoteText = "I have nothing to declare" },
            new Quote { QuoteText = "To be or not to be" },
        };

        var result = _service.FindShortestMatch(1, quotes, _regex);

        Assert.That(result.ShortestQuote, Is.EqualTo("I have a dream"));
    }

    [Test]
    public async Task FindShortestMatch_NoMatches_ReturnNull()
    {
        _regex = new Regex(@"\bhave\b", RegexOptions.IgnoreCase);

        var quotes = new List<Quote>
        {
            new Quote { QuoteText = "To be or not to be" },
        };

        var result = _service.FindShortestMatch(1, quotes, _regex);

        Assert.That(result.ShortestQuote, Is.Null);
    }

    [Test]
    public async Task FindShortestMatch_WholeWordOnly_DoesNotMatchPartial()
    {
        _regex = new Regex(@"\bcat\b", RegexOptions.IgnoreCase);

        var quotes = new List<Quote>
        {
            new Quote { QuoteText = "This is a category" },
            new Quote { QuoteText = "This is my cat" },
        };

        var result = _service.FindShortestMatch(1, quotes, _regex);

        Assert.That(result.ShortestQuote, Is.EqualTo("This is my cat"));
    }

    [Test]
    public async Task FindShortestMatch_EmptyList_ReturnsNull()
    {
        _regex = new Regex(@"\bcat\b", RegexOptions.IgnoreCase);

        var result = _service.FindShortestMatch(1, new List<Quote>(), _regex);

        Assert.That(result.ShortestQuote, Is.Null);
    }

    [Test]
    public async Task FindShortestMatch_PreservesPageNumber()
    {
        _regex = new Regex(@"\btest\b", RegexOptions.IgnoreCase);

        var quotes = new List<Quote>
        {
            new Quote { QuoteText = "This is a test" },
        };

        var result = _service.FindShortestMatch(42, quotes, _regex);

        Assert.That(result.PageNumber, Is.EqualTo(42));
    }

}