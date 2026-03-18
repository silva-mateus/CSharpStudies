using Assignment_Quote_Finder.Models;
using System.Text.RegularExpressions;

namespace Assignment_Quote_Finder;

public class QuoteSearchService
{
    public PageResult FindShortestMatch(int pageNumber, List<Quote> quotes, Regex regex)
    {
        var shortestMatch = quotes
                    .Where(q => regex.IsMatch(q.QuoteText))
                    .OrderBy(q => q.QuoteText.Length)
                    .FirstOrDefault();

        return new PageResult(pageNumber, shortestMatch?.QuoteText);
    }
}
