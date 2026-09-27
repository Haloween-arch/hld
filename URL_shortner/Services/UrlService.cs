using UrlShortener.Utils;
using UrlShortener.Models;
using UrlShortener.Repositories;
namespace UrlShortener.Services;
public class UrlService : IUrlService
{
    private readonly IUrlRepository _urlRepository;
    private readonly ICacheService _cacheService;

    public UrlService(IUrlRepository urlRepository, ICacheService cacheService)
    {
        _urlRepository = urlRepository;
        _cacheService = cacheService;
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
        var cacheKey = $"url:{shortCode}";

        // 1. Check Redis
        var cachedUrl = await _cacheService.GetAsync(cacheKey);

        if (cachedUrl != null)
        {
            // Cache HIT
            await _urlRepository.IncrementClickCountAsync(shortCode);

            return cachedUrl;
        }

        // 2. Cache MISS → PostgreSQL
        var url = await _urlRepository.GetByShortCodeAsync(shortCode);

        if (url == null)
        {
            return null;
        }

        // 3. Store URL in Redis
        await _cacheService.SetAsync(
            cacheKey,
            url.OriginalUrl,
            TimeSpan.FromHours(1)
        );

        // 4. Count the click
        await _urlRepository.IncrementClickCountAsync(shortCode);

        return url.OriginalUrl;
    }

    public async Task<Url?> GetStatsAsync(string shortCode)
    {
        return await _urlRepository.GetByShortCodeAsync(shortCode);
    }

    public async Task<bool> UpdateUrlAsync(
    string shortCode,
    string originalUrl)
{
    var url = await _urlRepository
        .GetByShortCodeAsync(shortCode);

    if (url == null)
    {
        return false;
    }

    await _urlRepository.UpdateUrlAsync(
        shortCode,
        originalUrl);

    // Invalidate Redis
    await _cacheService.RemoveAsync(
        $"url:{shortCode}");

    return true;
}

    private string GenerateShortCode()
    {
        // Implement a method to generate a unique short code
        return Base62Generator.Encode(DateTime.UtcNow.Ticks);
    }
}