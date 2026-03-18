using Assignment_Quote_Finder.DataAccess;
using Assignment_Quote_Finder.Models;
using Assignment_Quote_Finder.UserCommunication;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Assignment_Quote_Finder;

public class QuoteFinderApp
{
    private readonly IUserCommunication _uc;
    private readonly IQuotesApiDataReader _dataReader;
    private readonly QuoteSearchService _searchService;

    public QuoteFinderApp(IUserCommunication uc, IQuotesApiDataReader dataReader, QuoteSearchService searchService)
    {
        _uc = uc;
        _dataReader = dataReader;
        _searchService = searchService;
    }

    public async Task RunAsync(CancellationToken token = default)
    {
        var word = _uc.ReadWord("Insert the word you want to search:");
        var pages = _uc.ReadInt("Insert the number of pages to check:");
        var quotesPerPage = _uc.ReadInt("Insert the number of quotes per page in the search.");
        var isParallelProcess = _uc.ReadBool("Do you want to process in parallel or not? 'y' for Yes, 'n' for No");

        var pattern = $@"\b{Regex.Escape(word)}\b";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

        List<PageResult> pageResults;

        var stopwatch = Stopwatch.StartNew();
        if (isParallelProcess)
        {
            var tasks = Enumerable.Range(1, pages)
                .Select(p => ProcessPageAsync(p, quotesPerPage, regex,cts.Token))
                .ToList();

            try
            {
                var results = await Task.WhenAll(tasks);
                pageResults = results.ToList();
            }
            catch (OperationCanceledException)
            {
                _uc.PrintMessage("Operation was cancelled by the user.");
                return;
            }

        }
        else
        {
            pageResults = new List<PageResult>();
            for (int i = 1; i <= pages; i++)
            {
                cts.Token.ThrowIfCancellationRequested();
                pageResults.Add(await ProcessPageAsync(i, quotesPerPage, regex, cts.Token));
            }

        }
        stopwatch.Stop();

        foreach (var result in pageResults.OrderBy(r => r.PageNumber))
        {
            if (result.ShortestQuote != null)
            {
                _uc.PrintMessage($"Page {result.PageNumber}: \"{result.ShortestQuote}\" (length: {result.ShortestQuote.Length}).");
            }
            else
            {
                _uc.PrintMessage($"Page {result.PageNumber}: no quotes found with the word '{word}'.");
            }
        }

        var totalFound = pageResults.Count(r => r.ShortestQuote != null);
        _uc.PrintMessage($"\nFound Matches in {totalFound} out of {pages} pages.");
        _uc.PrintMessage($"Process took {stopwatch.ElapsedMilliseconds} milliseconds.");
        _uc.PrintMessage("Program is finished.");

    }

    private async Task<PageResult> ProcessPageAsync(int pageNumber, int quotesPerPage, Regex regex, CancellationToken token)
    {
        try
        {
            token.ThrowIfCancellationRequested();

            var jsonResponse = await _dataReader.ReadAsync(pageNumber, quotesPerPage);
            var response = JsonSerializer.Deserialize<QuoteResponse>(jsonResponse);

            if (response?.Data != null)
            {
                return _searchService.FindShortestMatch(pageNumber, response.Data, regex);
            }
            return new PageResult(pageNumber, null);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {

            _uc.PrintMessage($"Error processing page {pageNumber}: {ex.Message}");
            return new PageResult(pageNumber, null);
        }
    }


}


