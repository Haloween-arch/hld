namespace UrlShortener.Models;
public class Url
{
    public long Id { get; set; }

    public string OriginalUrl { get; set; } = null!;

    public string ShortCode { get; set; } = null!;

    public long ClickCount { get; set; }

    public DateTime CreatedAt { get; set; }
}