using IX02_Generics_GenericCache;

var cache = new GenericCache<string, int>();

cache.Add("score", 100, TimeSpan.FromMinutes(5));
cache.Add("lives", 3, TimeSpan.FromMinutes(10));

if (cache.TryGet("score", out var score))
{
    Console.WriteLine($"Score: {score}");
}

Console.WriteLine($"Cache entries: {cache.Count}");

cache.Remove("score");
Console.WriteLine($"After removal: {cache.Count}");
