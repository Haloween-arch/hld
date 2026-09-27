using StackExchange.Redis;

namespace UrlShortener.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase _database;

    public CacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<string?> GetAsync(string key)
    {
        return await _database.StringGetAsync(key);
    }

    public async Task SetAsync(
        string key,
        string value,
        TimeSpan expiry)
    {
        await _database.StringSetAsync(
            key,
            value,
            expiry
        );
    }
    public async Task RemoveAsync(string key)
{
    await _database.KeyDeleteAsync(key);
}
}