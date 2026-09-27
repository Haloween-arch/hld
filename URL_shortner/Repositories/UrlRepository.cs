using UrlShortener.Models;
using UrlShortener.Data;
using Microsoft.EntityFrameworkCore;
namespace UrlShortener.Repositories;
public class UrlRepository : IUrlRepository
{
    private readonly AppDbContext _dbContext;
    public UrlRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Url?> GetByShortCodeAsync(string shortCode)
    {
        return await _dbContext.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
    }

    public async Task<Url> CreateAsync(Url url)
    {
        _dbContext.Urls.Add(url);
        await _dbContext.SaveChangesAsync();
        return url;
    }

    public async Task IncrementClickCountAsync(string shortCode)
    {
    await _dbContext.Urls
        .Where(u => u.ShortCode == shortCode)
        .ExecuteUpdateAsync(setters =>
            setters.SetProperty(
                u => u.ClickCount,
                u => u.ClickCount + 1
            ));
     }
     public async Task UpdateUrlAsync(
    string shortCode,
    string originalUrl)
{
    await _dbContext.Urls
        .Where(u => u.ShortCode == shortCode)
        .ExecuteUpdateAsync(setters =>
            setters.SetProperty(
                u => u.OriginalUrl,
                originalUrl
            ));
}
}