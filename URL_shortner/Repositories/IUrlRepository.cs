using UrlShortener.Models;
namespace UrlShortener.Repositories;
public interface IUrlRepository
{
    Task<Url?> GetByShortCodeAsync(string shortCode);

    Task<Url> CreateAsync(Url url);

    Task IncrementClickCountAsync(string shortCode);
}