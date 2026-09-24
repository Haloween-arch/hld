using UrlShortener.Models;
namespace UrlShortener.Services;
public interface IUrlService
{
    Task<Url> CreateUrlAsync(string originalUrl);

    Task<string?> GetOriginalUrlAsync(string shortCode);

    Task<Url?> GetStatsAsync(string shortCode);
}