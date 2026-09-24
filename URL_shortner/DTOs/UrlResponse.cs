namespace UrlShortener.DTOs;

public class UrlResponse
{
    public string ShortCode { get; set; } = null!;

    public string ShortUrl { get; set; } = null!;
}