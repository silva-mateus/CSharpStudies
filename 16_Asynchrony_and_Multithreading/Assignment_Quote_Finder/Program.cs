using Assignment_Quote_Finder.DataAccess.Mock;
using Assignment_Quote_Finder.UserCommunication;

namespace Assignment_Quote_Finder;

public class Program
{
    public static async Task Main(string[] args)
    {
        var uc = new ConsoleCommunication();
        var dataReader = new MockQuotesApiDataReader();

        var app = new QuoteFinderApp(uc, dataReader, new QuoteSearchService());
        await app.RunAsync();
        Console.ReadKey();
    }
}