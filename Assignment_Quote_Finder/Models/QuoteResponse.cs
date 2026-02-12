using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Assignment_Quote_Finder.Models;

public record QuoteResponse
{
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; init; }

    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("pagination")]
    public string? Pagination { get; init; }

    [JsonPropertyName("totalQuotes")]
    public int TotalQuotes { get; init; }

    [JsonPropertyName("data")]
    public List<Quote> Data { get; init; } = [];

}

public record Quote
{

    [JsonPropertyName("_id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("quoteText")]
    public string QuoteText{ get; init; } = string.Empty;

    [JsonPropertyName("quoteAuthor")]
    public string QuoteAuthor { get; init; } = string.Empty;

    [JsonPropertyName("quoteGenre")]
    public string? QuoteGenre { get; init; }

}

