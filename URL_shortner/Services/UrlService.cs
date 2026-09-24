using UrlShortener.Utils;
using UrlShortener.Models;
using UrlShortener.Repositories;
namespace UrlShortener.Services;
public class UrlService : IUrlService
{
    private readonly IUrlRepository _urlRepository;

    public UrlService(IUrlRepository urlRepository)
    {
        _urlRepository = urlRepository;
    }

    public async Task<Url> CreateUrlAsync(string originalUrl)
    {
        var shortCode = GenerateShortCode();
        var url = new Url
        {
            OriginalUrl = originalUrl,
            ShortCode = shortCode,
            ClickCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        return await _urlRepository.CreateAsync(url);
    }

    public async Task<string?> GetOriginalUrlAsync(string shortCode)
    {
        var url = await _urlRepository.GetByShortCodeAsync(shortCode);
        if (url != null)
        {
            await _urlRepository.IncrementClickCountAsync(shortCode);
            return url.OriginalUrl;
        }
        return null;
    }

    public async Task<Url?> GetStatsAsync(string shortCode)
    {
        return await _urlRepository.GetByShortCodeAsync(shortCode);
    }

    private string GenerateShortCode()
    {
        // Implement a method to generate a unique short code
        return Base62Generator.Encode(DateTime.UtcNow.Ticks);
    }
}